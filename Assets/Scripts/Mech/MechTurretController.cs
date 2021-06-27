using Pathfinding;
using UnityEngine;

public class MechTurretController : MonoBehaviour
{
    [SerializeField]
    private MechTurretRotationController turretRotation;
    [SerializeField]
    private MechWeaponsController mechWeapons;
    [SerializeField]
    private WeaponTargetRepository targetRepository;
    [SerializeField]
    private AIPath pathfinder;

    private Vector3 lookDirection;

    public void SetDirection(Vector3 direction)
    {
        lookDirection = direction;
        if (targetRepository.PriorityTarget == null || !mechWeapons.Armed)
        {
            turretRotation.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (targetRepository.PriorityTarget != null && mechWeapons.Armed)
        {
            turretRotation.LookAt(mechWeapons.GetPriorityDirection());
        }
        else if (targetRepository.PriorityTarget == null && pathfinder.reachedEndOfPath)
        {
            turretRotation.LookAt(lookDirection);
        }
    }
}
