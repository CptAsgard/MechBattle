using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField]
    private ProjectileWeaponData weaponData;
    [SerializeField]
    private WeaponFireController fireController;
    [SerializeField]
    private GameObject projectile;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => fireController.ReadyToFire;

    private void Update()
    {
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
        float? highAngle, lowAngle = 0f;
        CalculateAngleToHitTarget(out highAngle, out lowAngle);

        if (lowAngle != null || highAngle != null)
        {
            float angle = (float) (lowAngle ?? highAngle);

            Origin.LookAt(target.Current);
            Origin.localEulerAngles = new Vector3(360f - angle, Origin.localEulerAngles.y, Origin.localEulerAngles.z);
        }
    }

    void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
    {
        //Initial speed
        float v = weaponData.muzzleVelocity;

        Vector3 targetVec = target.Current.position - Origin.position;

        //Vertical distance
        float y = targetVec.y;

        //Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

        //Horizontal distance
        float x = targetVec.magnitude;

        //Gravity
        float g = -Physics.gravity.y;

        //Calculate the angles
        float vSqr = v * v;

        float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

        //Check if we are within range
        if (underTheRoot >= 0f)
        {
            float rightSide = Mathf.Sqrt(underTheRoot);

            float top1 = vSqr + rightSide;
            float top2 = vSqr - rightSide;

            float bottom = g * x;

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
