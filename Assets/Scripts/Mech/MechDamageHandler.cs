using Pathfinding;
using UnityEngine;

public class MechDamageHandler : MonoBehaviour, IDamageable
{
    [SerializeField]
    private MechComponentRepository componentRepository;
    [SerializeField]
    private MechState mechState;
    [SerializeField]
    private AIPath aiPath;

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
            aiPath.isStopped = true;
            aiPath.canMove = false;

            GetComponent<MechTurretAngleController>().enabled = false;
            GetComponent<MechWeaponsController>().enabled = false;
            GetComponent<MechTargetingController>().enabled = false;

            Debug.Log("MECH DESTROYED!", gameObject);
        }
    }
}
