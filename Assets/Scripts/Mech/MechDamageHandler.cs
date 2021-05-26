using UnityEngine;

public class MechDamageHandler : MonoBehaviour, IDamageable
{
    [SerializeField]
    private MechComponentRepository componentRepository;
    [SerializeField]
    private MechState mechState;

    public void TakeDamage(MechComponentLocation location, IDamageForce force)
    {
        Debug.Log($"{name} damaged. Health " +
            $"(old): {componentRepository.GetComponent(location).Health} (new): {componentRepository.GetComponent(location).Health - force.Damage}");

        componentRepository.GetComponent(location).Health -= force.Damage;

        CheckDestructionState();
    }

    private void CheckDestructionState()
    {
        if (componentRepository.GetComponent(MechComponentLocation.Torso).Health <= 0)
        {
            mechState.PowerState = MechPowerState.Destroyed;
            Debug.Log("MECH DESTROYED!", gameObject);
        }
    }
}
