using Mirror;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField]
    private ProjectileWeaponData weaponData;
    [SerializeField]
    private ProjectileWeaponView weaponView;
    [SerializeField]
    private WeaponReloadDelay reloadDelay;
    [SerializeField]
    private Transform muzzleEnd;
    [SerializeField]
    private Projectile projectileClient;
    [SerializeField]
    private Projectile projectileServer;

    private WeaponTargetRepository targetRepository;
    private bool inRange;
    private ProjectileData projectileData;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => reloadDelay.ReadyToFire && inRange;

    private void FixedUpdate()
    {
        if (!isServer)
        {
            enabled = false;
            return;
        }

        if (!Owner)
        {
            return;
        }

        Aim();
    }

    public override void Initialize(MechState owner, Transform parent)
    {
        base.Initialize(owner, parent);

        targetRepository = owner.GetComponent<WeaponTargetRepository>();
        projectileData = projectileServer.ProjectileData;
    }

    [Server]
    public override void Fire()
    {
        if (!Armed)
        {
            return;
        }

        reloadDelay.ResetCooldown();

        Vector3 position = muzzleEnd.position;
        Vector3 forward = muzzleEnd.forward;

        SpawnBullet(projectileServer.gameObject, position, forward);
        RpcSpawnBullet(position, forward);
    }

    [ClientRpc]
    private void RpcSpawnBullet(Vector3 position, Vector3 forward)
    {
        SpawnBullet(projectileClient.gameObject, position, forward);
    }
    
    private static void SpawnBullet(GameObject prefab, Vector3 position, Vector3 forward)
    {
        GameObject pr = Instantiate(prefab);
        pr.GetComponent<Projectile>().Initialize(position, forward);
    }

    private void Aim()
    {
        if (targetRepository.PriorityTarget == null)
        {
            inRange = false;
            return;
        }

        Vector3 targetPositionWorld = targetRepository.PriorityTarget.GetComponent<MechComponentRepository>().GetWorldPosition(MechComponentLocation.Torso);

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
    }

    private void CalculateAngleToHitTarget(Vector3 target, out float? theta1, out float? theta2)
    {
        float velocity = projectileData.MuzzleVelocity;
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
