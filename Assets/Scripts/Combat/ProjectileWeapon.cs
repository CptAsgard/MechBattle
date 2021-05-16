using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField]
    private ProjectileWeaponData weaponData;
    [SerializeField]
    private WeaponFireController fireController;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private LineRenderer lineRenderer;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => fireController.ReadyToFire;

    private void Update()
    {
        if (MechData.target == null)
        {
            return;
        }

        Aim();
    }

    public override void Fire()
    {
        if (!Armed)
        {
            return;
        }

        GameObject pr = Instantiate(projectile);
        pr.GetComponent<Projectile>().Initialize(Origin.position, Origin.forward, weaponData);

        fireController.ResetCooldown();
    }

    private void Aim()
    {
        if (MechData.target == null)
        {
            InRange = false;
            return;
        }

        CalculateAngleToHitTarget(out var highAngle, out var lowAngle);
        InRange = lowAngle != null || highAngle != null;

        if (lowAngle == null && highAngle == null)
        {
            AimDirection = (MechData.target.position - Origin.position).normalized;
            return;
        }

        float angle = (float) (lowAngle ?? highAngle);

        Origin.LookAt(MechData.target);
        Origin.localEulerAngles = new Vector3(360f - angle, Origin.localEulerAngles.y, Origin.localEulerAngles.z);
        AimDirection = Origin.forward;
    }

    void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
    {
        float velocity = weaponData.muzzleVelocity;
        Vector3 targetVector = MechData.target.position - Origin.position;
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
