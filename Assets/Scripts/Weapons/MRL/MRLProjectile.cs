using System.Collections.Generic;
using UnityEngine;

public class MRLProjectile : Projectile
{
    [SerializeField]
    private MRLProjectileData projectileData;

    private Transform target;
    private float flightTime;
    private float initialDistance;
    private float randomX, randomZ;

    public override ProjectileData ProjectileData => projectileData;

    public override void Initialize(Vector3 initialPosition, bool withAuthority)
    {
        base.Initialize(initialPosition, withAuthority);

        if (hasAuthority)
        {
            OnHitEvent += OnHit;
        }
    }

    public void Initialize(Vector3 initialPosition, Vector3 forward, Transform target, bool withAuthority)
    {
        base.Initialize(initialPosition, withAuthority);
        this.target = target;

        transform.position = initialPosition;
        transform.forward = forward;
        
        initialDistance = (target.position - transform.position).magnitude;        
        
        if (hasAuthority)
        {
            OnHitEvent += OnHit;
        }
    }

    private void FixedUpdate()
    {
        flightTime += Time.fixedDeltaTime;

        Vector3 targetVector = target.position - transform.position;
        Vector3 targetDirection = (target.position - transform.position).normalized;

        randomX = Mathf.Clamp(randomX + Random.Range(-projectileData.RandomConeAngle / 15f, projectileData.RandomConeAngle / 15f),
            -projectileData.RandomConeAngle, projectileData.RandomConeAngle);
        randomZ = Mathf.Clamp(randomZ + Random.Range(-projectileData.RandomConeAngle / 15f, projectileData.RandomConeAngle / 15f),
            -projectileData.RandomConeAngle, projectileData.RandomConeAngle);

        Vector3 randomDirection = Quaternion.Euler(randomX, 0, randomZ) * targetDirection;
        Vector3 modulatedDirection = Vector3.Slerp(randomDirection, targetDirection,
            (initialDistance - targetVector.magnitude) / initialDistance);

        Quaternion newRotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, modulatedDirection,
            projectileData.RotationSpeed * projectileData.TurnControlCurve.Evaluate(flightTime) * Time.fixedDeltaTime, 0f));

        Vector3 newPosition = transform.position + projectileData.MuzzleVelocity * Time.fixedDeltaTime * transform.forward;

        if (Physics.Linecast(currentPosition, newPosition, out RaycastHit hitInfo, layerMask))
        {
            newPosition = hitInfo.point;

            var hits = Physics.OverlapSphere(transform.position, projectileData.ExplosionRadius, layerMask);
            var damageables = new List<IDamageable>(hits.Length);
            foreach (Collider hit in hits)
            {
                IDamageable newDamageable = hit.GetComponentInParent<IDamageable>();
                if (newDamageable == null || damageables.Contains(newDamageable))
                {
                    continue;
                }
                damageables.Add(newDamageable);
                OnHitEvent?.Invoke(newDamageable, transform.position);
            }

            Destroy(gameObject);
        }

        transform.rotation = newRotation;
        transform.position = newPosition;
    }

    private void OnHit(IDamageable damageable, Vector3 point)
    {
        damageable.TakeDamage(point, new DamageForce(ProjectileData.DamageOnHit));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, projectileData.ExplosionRadius);
    }
}
