using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private EnvironmentConstants environment;
    [SerializeField]
    private TrailRenderer trail;

    private Vector3 currentPosition;
    private Vector3 currentVelocity;

    private void FixedUpdate()
    {
        StepBullet();
        trail.enabled = true;
    }

    public void Initialize(Vector3 initialPosition, Vector3 direction, ProjectileWeaponData weaponData)
    {
        currentPosition = initialPosition;
        currentVelocity = direction * weaponData.muzzleVelocity;
    }

    private void StepBullet()
    {
        ProjectileIntegrationMethods.HeunsNoExternalForces(Time.fixedDeltaTime, currentPosition, currentVelocity, out Vector3 newPosition, out Vector3 newVelocity);

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        transform.position = currentPosition;
        transform.forward = currentVelocity.normalized;
    }
}