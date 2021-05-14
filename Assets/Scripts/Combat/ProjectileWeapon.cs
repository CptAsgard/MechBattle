using System.Collections.Generic;
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
        if (Target.Current == null)
        {
            return;
        }

        Aim();
        DrawTrajectoryPath();
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
        if (Target.Current == null)
        {
            WithinRange = false;
            return;
        }

        CalculateAngleToHitTarget(out var highAngle, out var lowAngle);
        WithinRange = lowAngle != null || highAngle != null;

        if (lowAngle == null && highAngle == null)
        {
            return;
        }

        float angle = (float) (lowAngle ?? highAngle);
        Origin.LookAt(Target.Current);
        Origin.localEulerAngles = new Vector3(360f - angle, Origin.localEulerAngles.y, Origin.localEulerAngles.z);
        AimDirection = Origin.forward;
    }

    void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
    {
        //Initial speed
        float v = weaponData.muzzleVelocity;

        Vector3 targetVec = Target.Current.position - Origin.position;

        //Vertical distance
        float y = targetVec.y;

        //Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

        //Horizontal distance
        float x = targetVec.magnitude;

        //Gravity
        float g = Mathf.Abs(Physics.gravity.y);


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



    //Display the trajectory path with a line renderer
    void DrawTrajectoryPath()
    {
        //Start values
        Vector3 currentVel = Origin.forward * weaponData.muzzleVelocity;
        Vector3 currentPos = Origin.position;

        Vector3 newPos = Vector3.zero;
        Vector3 newVel = Vector3.zero;

        List<Vector3> bulletPositions = new List<Vector3>();

        //Build the trajectory line
        bulletPositions.Add(currentPos);

        //I prefer to use a maxIterations instead of a while loop 
        //so we always avoid stuck in infinite loop and have to restart Unity
        //You might have to change this value depending on your values
        int maxIterations = 10000;

        for (int i = 0; i < maxIterations; i++)
        {
            //Calculate the bullets new position and new velocity
            CurrentIntegrationMethod(Time.fixedDeltaTime, currentPos, currentVel, out newPos, out newVel);

            //Set the new value to the current values
            currentPos = newPos;
            currentVel = newVel;

            //Add the new position to the list with all positions
            bulletPositions.Add(currentPos);

            //The bullet has hit the ground because we assume 0 is ground height
            //This assumes the bullet is fired from a position above 0 or the loop will stop immediately
            if (currentPos.y < 0f)
            {
                break;
            }

            //A warning message that something might be wrong
            if (i == maxIterations - 1)
            {
                Debug.Log("The bullet newer hit anything because we reached max iterations");
            }
        }


        //Display the bullet positions with a line renderer
        lineRenderer.positionCount = bulletPositions.Count;

        lineRenderer.SetPositions(bulletPositions.ToArray());
    }



    //Choose which integration method you want to use to simulate how the bullet fly
    public static void CurrentIntegrationMethod(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
    {
        //IntegrationMethods.BackwardEuler(timeStep, currentPos, currentVel, out newPos, out newVel);

        //IntegrationMethods.ForwardEuler(timeStep, currentPos, currentVel, out newPos, out newVel);

        //IntegrationMethods.Heuns(timeStep, currentPos, currentVel, out newPos, out newVel);

        ProjectileIntegrationMethods.HeunsNoExternalForces(timeStep, currentPos, currentVel, out newPos, out newVel);
    }
}
