using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MRLProjectile : MonoBehaviour
{
    public System.Action<IDamageable, Vector3> OnHitEvent;

    [SerializeField]
    private MRLProjectileData projectileData;
    [SerializeField]
    private LayerMask layerMask;

    private Transform target;
    private float flightTime;
    private Vector3 currentPosition;
    private float initialDistance;
    private float randomX, randomZ;

    public MRLProjectileData ProjectileData => projectileData;

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

    public void Initialize(Vector3 position, Vector3 forward, Transform target)
    {
        this.target = target;

        transform.position = position;
        transform.forward = forward;

        currentPosition = position;
        initialDistance = (target.position - transform.position).magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, projectileData.ExplosionRadius);
    }
}
