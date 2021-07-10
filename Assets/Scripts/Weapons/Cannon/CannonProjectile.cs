using UnityEngine;

public class CannonProjectile : Projectile
{
    [SerializeField]
    private ProjectileData projectileData;

    private Vector3 currentVelocity;
    
    public override ProjectileData ProjectileData => projectileData;

    private void FixedUpdate()
    {
        StepBullet();
        OnStepEvent?.Invoke(currentPosition);

        if (sinceSpawn > 10f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 initialPosition, Vector3 direction, bool withAuthority)
    {
        base.Initialize(initialPosition, withAuthority);
        currentVelocity = direction * projectileData.MuzzleVelocity;

        if (hasAuthority)
        {
            OnHitEvent += OnHit;
        }
    }

    private void OnHit(IDamageable damageable, Vector3 point)
    {
        damageable.TakeDamage(point, new DamageForce(ProjectileData.DamageOnHit));
    }

    private void StepBullet()
    {
        ProjectileIntegrationMethods.HeunsNoExternalForces(Time.fixedDeltaTime, currentPosition, currentVelocity, out Vector3 newPosition, out Vector3 newVelocity);

        if (Physics.Linecast(currentPosition, newPosition, out RaycastHit hitInfo, layerMask))
        {
            newPosition = hitInfo.point;

            IDamageable damageable = hitInfo.transform.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                OnHitEvent?.Invoke(damageable, newPosition);
            }
            Destroy(gameObject);
        }

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        transform.position = currentPosition;
        transform.forward = currentVelocity.normalized;
    }
}
