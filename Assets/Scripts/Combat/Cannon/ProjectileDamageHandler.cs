using UnityEngine;

public class ProjectileDamageHandler : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    private void Awake()
    {
        projectile.OnHitEvent += OnHitEvent;
    }

    private void OnHitEvent(IDamageable damageable, Vector3 point)
    {
        damageable.TakeDamage(point, new DamageForce(projectile.ProjectileData.DamageOnHit));
    }
}
