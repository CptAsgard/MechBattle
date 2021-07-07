public class DamageForce : IDamageForce
{
    public float Damage { get; private set; }

    public DamageForce(float damage)
    {
        Damage = damage;
    }
}
