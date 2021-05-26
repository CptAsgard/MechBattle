using Mirror;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField]
    private ProjectileWeaponData weaponData;
    [SerializeField]
    private ProjectileWeaponView weaponView;
    [SerializeField]
    private WeaponFireController fireController;
    [SerializeField]
    private GameObject projectile;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => fireController.ReadyToFire;

    private void Update()
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

    [Server]
    public override void Fire()
    {
        if (!Armed)
        {
            return;
        }

        fireController.ResetCooldown();

        Vector3 position = Origin.position;
        Vector3 forward = Origin.forward;

        SpawnBullet(position, forward);
        weaponView.RpcFire(position, forward);
    }
    
    [Server]
    private void SpawnBullet(Vector3 position, Vector3 forward)
    {
        GameObject pr = Instantiate(projectile);
        pr.GetComponent<Projectile>().Initialize(position, forward, weaponData, OnProjectileHit);
    }

    [Server]
    private static void OnProjectileHit(IDamageable damageable)
    {
        damageable?.TakeDamage(MechComponentLocation.Torso, new DamageForce(20));
    }

    private void Aim()
    {
        if (Owner.Target == null)
        {
            InRange = false;
            return;
        }

        Vector3 targetPositionWorld = Owner.Target.GetComponent<MechComponentRepository>().GetWorldPosition(MechComponentLocation.Torso);

        CalculateAngleToHitTarget(targetPositionWorld, out var highAngle, out var lowAngle);
        InRange = lowAngle != null || highAngle != null;

        if (lowAngle == null && highAngle == null)
        {
            AimDirection = (targetPositionWorld - Origin.position).normalized;
            return;
        }

        float angle = (float) (lowAngle ?? highAngle);

        Origin.LookAt(targetPositionWorld);
        Origin.localEulerAngles = new Vector3(360f - angle, Origin.localEulerAngles.y, Origin.localEulerAngles.z);
        AimDirection = Origin.forward;
    }
    
    private void CalculateAngleToHitTarget(Vector3 target, out float? theta1, out float? theta2)
    {
        float velocity = weaponData.muzzleVelocity;
        Vector3 targetVector = target - Origin.position;
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
