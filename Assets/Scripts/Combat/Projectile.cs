using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private LayerMask layerMask;

    private Vector3 currentPosition;
    private Vector3 currentVelocity;
    private System.Action<IDamageable, Vector3> onHitAction;
    private float timer;

    private void FixedUpdate()
    {
        StepBullet();
        trail.emitting = true;

        timer += Time.fixedDeltaTime;
        if (timer >= 10f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 initialPosition, Vector3 direction, ProjectileWeaponData weaponData, System.Action<IDamageable, Vector3> callback = null)
    {
        currentPosition = initialPosition;
        currentVelocity = direction * weaponData.muzzleVelocity;
        onHitAction = callback;
        
        trail.AddPosition(currentPosition);
    }

    private void StepBullet()
    {
        ProjectileIntegrationMethods.HeunsNoExternalForces(Time.fixedDeltaTime, currentPosition, currentVelocity, out Vector3 newPosition, out Vector3 newVelocity);

        if (Physics.Linecast(currentPosition, newPosition, out RaycastHit hitInfo, layerMask))
        {
            newPosition = hitInfo.point;

            IDamageable damageable = hitInfo.transform.GetComponentInParent<IDamageable>();
            onHitAction?.Invoke(damageable, newPosition);
            Destroy(gameObject);
        }

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        transform.position = currentPosition;
        transform.forward = currentVelocity.normalized;
    }

    private void OnDestroy()
    {
        trail.transform.parent = null;
        trail.autodestruct = true;
    }
}
