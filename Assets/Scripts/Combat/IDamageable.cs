using UnityEngine;

public interface IDamageForce
{
    float Damage { get; }
}

public interface IDamageable
{
    void TakeDamage(Vector3 point, IDamageForce force);
}
