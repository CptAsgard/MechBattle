using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using NetworkIdentity = Mirror.NetworkIdentity;

public class CannonWeaponController : WeaponController
{
    [SerializeField]
    private WeaponData weaponData;
    [SerializeField]
    private TurretRotationController turretRotation;
    [SerializeField]
    private Transform muzzleEnd;
    [SerializeField]
    private CannonProjectile projectileClient;
    [SerializeField]
    private CannonProjectile projectileServer;
    [SerializeField]
    private float fireAngleThreshold;

    private NetworkIdentity target;
    private bool inRange;
    private float muzzleVelocity;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => reloadTime > weaponData.ReloadDelay && inRange;
    public override bool AutoAim => true;

    public override void Initialize(GameObject mech, WeaponAttachmentPoint attachmentPoint)
    {
        base.Initialize(mech, attachmentPoint);

        muzzleVelocity = projectileServer.ProjectileData.MuzzleVelocity;
        transform.localPosition = Vector3.zero;
    }

    [Server]
    private void FixedUpdate()
    {
        UpdateTargets();

        if (!WeaponOwner.Owner || target == null)
        {
            if (LineOfSightIgnoredRepository.Instance.Contains(netIdentity))
            {
                LineOfSightIgnoredRepository.Instance.Remove(netIdentity);
            }
            return;
        }

        Aim();

        if (weaponData.ReloadDelay - reloadTime <= 1f && !LineOfSightIgnoredRepository.Instance.Contains(netIdentity))
        {
            LineOfSightIgnoredRepository.Instance.Add(netIdentity);
        }

        if (!Armed)
        {
            return;
        }

        if (Vector3.Angle(transform.forward, AimDirection) > fireAngleThreshold)
        {
            return;
        }

        Fire();
        LineOfSightIgnoredRepository.Instance.Remove(netIdentity);
    }


    [Server]
    private void UpdateTargets()
    {
        if (targetRepository.PriorityTarget != null)
        {
            target = targetRepository.PriorityTarget;
            return;
        }
        
        //var friendlyMechs =
        //    MechRepository.Instance.Mechs.GetFriendly(WeaponOwner.Owner.PlayerIndex).Select(mech => mech.GetComponent<MechVisibilityHandler>()).ToList();
        //friendlyMechs.Any(mech => mech.Visible.Contains(test))

        List<NetworkIdentity> newTargets = new List<NetworkIdentity>();
        foreach (NetworkIdentity test in TargetsRepository.Instance.Targets)
        {
            if (WeaponOwner.Owner.GetComponent<MechVisibilityHandler>().Visible.Contains(test))
            {
                newTargets.Add(test);
            }
        }
        
        var newTarget = newTargets.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
        
        if (newTarget != target && targetRepository.Contains(target))
        {
            targetRepository.Remove(target);
        }

        if (!targetRepository.Contains(newTarget))
        {
            targetRepository.Add(newTarget);
        }

        target = newTarget;
    }

    [Server]
    public override void Fire()
    {
        if (!Armed)
        {
            return;
        }

        reloadTime = 0f;

        Vector3 position = muzzleEnd.position;
        Vector3 forward = muzzleEnd.forward;

        SpawnBullet(projectileServer.gameObject, position, forward, true);
        RpcSpawnBullet(position, forward);
    }

    [Server]
    protected override void Aim()
    {
        if (target == null)
        {
            inRange = false;
            return;
        }

        Vector3 targetPositionWorld = target.GetComponent<MechComponentRepository>().GetWorldPosition(MechComponentLocation.Torso);

        CalculateAngleToHitTarget(targetPositionWorld, out float? highAngle, out float? lowAngle);
        inRange = lowAngle != null || highAngle != null;

        if (!inRange)
        {
            return;
        }

        // ReSharper disable once PossibleInvalidOperationException because we already check inRange, one of them -has- to be not null.
        float angle = (float) (lowAngle ?? highAngle);

        muzzleEnd.LookAt(targetPositionWorld);
        muzzleEnd.eulerAngles = new Vector3(360f - angle, muzzleEnd.eulerAngles.y, muzzleEnd.eulerAngles.z);

        AimDirection = muzzleEnd.forward;
        turretRotation.LookAt(AimDirection);
    }

    [ClientRpc]
    private void RpcSpawnBullet(Vector3 position, Vector3 forward)
    {
        SpawnBullet(projectileClient.gameObject, position, forward, false);
    }

    private static void SpawnBullet(GameObject prefab, Vector3 position, Vector3 forward, bool withAuthority)
    {
        GameObject pr = Instantiate(prefab);
        pr.GetComponent<CannonProjectile>().Initialize(position, forward, withAuthority);
    }

    private void CalculateAngleToHitTarget(Vector3 target, out float? theta1, out float? theta2)
    {
        float velocity = muzzleVelocity;
        Vector3 targetVector = target - muzzleEnd.position;
        float x = new Vector3(targetVector.x, 0, targetVector.z).magnitude;
        float gravity = -Physics.gravity.y;

        float velocitySqr = velocity * velocity;

        float underRoot = velocitySqr * velocitySqr - gravity * (gravity * x * x + 2 * targetVector.y * velocitySqr);

        //Check if we are within range
        if (underRoot >= 0f)
        {
            float rightSide = Mathf.Sqrt(underRoot);
            float top1 = velocitySqr + rightSide;
            float top2 = velocitySqr - rightSide;
            float bottom = gravity * x;
            theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
            theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
        }
        else
        {
            theta1 = null;
            theta2 = null;
        }
    }
}
