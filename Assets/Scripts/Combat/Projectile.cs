using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private LayerMask layerMask;

    private Vector3 currentPosition;
    private Vector3 currentVelocity;

    private float timer;

    private void FixedUpdate()
    {
        StepBullet();
        trail.enabled = true;

        timer += Time.fixedDeltaTime;
        if (timer >= 10f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 initialPosition, Vector3 direction, ProjectileWeaponData weaponData)
    {
        currentPosition = initialPosition;
        currentVelocity = direction * weaponData.muzzleVelocity;
        trail.AddPosition(currentPosition);
    }

    private void StepBullet()
    {
        ProjectileIntegrationMethods.HeunsNoExternalForces(Time.fixedDeltaTime, currentPosition, currentVelocity, out Vector3 newPosition, out Vector3 newVelocity);

        if (Physics.Linecast(currentPosition, newPosition, out RaycastHit hitInfo, layerMask))
        {
            IDamageable damageable = hitInfo.transform.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(MechComponentLocation.Torso, new DamageForce(20));
            Destroy(gameObject);
            return;
        }

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        transform.position = currentPosition;
        transform.forward = currentVelocity.normalized;
    }
}
