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

    public MRLProjectileData ProjectileData => projectileData;

    private void FixedUpdate()
    {
        flightTime += Time.fixedDeltaTime;
        
        Quaternion newRotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (target.position - transform.position).normalized,
            projectileData.RotationSpeed * projectileData.TurnControlCurve.Evaluate(flightTime) * Time.fixedDeltaTime, 0f));

        Vector3 newPosition = transform.position + projectileData.MuzzleVelocity * Time.fixedDeltaTime * transform.forward;
        
        if (Physics.Linecast(currentPosition, newPosition, out RaycastHit hitInfo, layerMask))
        {
            newPosition = hitInfo.point;

            Collider[] hits = Physics.OverlapSphere(transform.position, projectileData.ExplosionRadius, layerMask);
            List<IDamageable> damageables = new List<IDamageable>(hits.Length);
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, projectileData.ExplosionRadius);
    }
}
