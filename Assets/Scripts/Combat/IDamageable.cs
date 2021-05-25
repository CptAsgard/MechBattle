public interface IDamageForce
{
    float Damage { get; }
}

public interface IDamageable
{
    void TakeDamage(MechComponentLocation location, IDamageForce force);
}
