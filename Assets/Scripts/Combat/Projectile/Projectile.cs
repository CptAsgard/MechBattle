using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public System.Action<Vector3> OnStepEvent;
    public System.Action<IDamageable, Vector3> OnHitEvent;
    public UnityEvent<Vector3> OnSpawnEvent;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private ProjectileData projectileData;

    public ProjectileData ProjectileData => projectileData;

    private Vector3 currentPosition;
    private Vector3 currentVelocity;
    private float timer;

    private void FixedUpdate()
    {
        StepBullet();
        OnStepEvent?.Invoke(currentPosition);

        timer += Time.fixedDeltaTime;
        if (timer >= 10f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 initialPosition, Vector3 direction)
    {
        currentPosition = initialPosition;
        currentVelocity = direction * projectileData.MuzzleVelocity;

        OnSpawnEvent?.Invoke(initialPosition);
    }

    private void StepBullet()
    {
        ProjectileIntegrationMethods.HeunsNoExternalForces(Time.fixedDeltaTime, currentPosition, currentVelocity, out Vector3 newPosition, out Vector3 newVelocity);

        if (Physics.Linecast(currentPosition, newPosition, out RaycastHit hitInfo, layerMask))
        {
            newPosition = hitInfo.point;

            IDamageable damageable = hitInfo.transform.GetComponentInParent<IDamageable>();
            OnHitEvent?.Invoke(damageable, newPosition);
            Destroy(gameObject);
        }

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        transform.position = currentPosition;
        transform.forward = currentVelocity.normalized;
    }
}
