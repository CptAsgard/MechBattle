using UnityEngine;

public class MechDamageHandler : MonoBehaviour, IDamageable
{
    [SerializeField]
    private MechComponentRepository componentRepository;

    public void TakeDamage(MechComponentLocation location, IDamageForce force)
    {
        Debug.Log($"{name} damaged. Health " +
            $"(old): {componentRepository.GetComponent(location).Health} (new): {componentRepository.GetComponent(location).Health - force.Damage}");

        componentRepository.GetComponent(location).Health -= force.Damage;
    }
}
