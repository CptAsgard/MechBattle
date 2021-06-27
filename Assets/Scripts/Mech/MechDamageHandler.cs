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

    public void TakeDamage(Vector3 point, IDamageForce force)
    {
        MechComponentLocation location = componentRepository.GetNearestComponent(point);

        Debug.Log($"{name} component {location} damaged. Health " +
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

            GetComponent<MechTurretRotationController>().enabled = false;
            GetComponent<MechWeaponsController>().enabled = false;
            GetComponent<MechTurretController>().enabled = false;

            Debug.Log("MECH DESTROYED!", gameObject);
        }
    }
}
