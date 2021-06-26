using Pathfinding;
using UnityEngine;

public class MechTurretController : MonoBehaviour
{
    [SerializeField]
    private MechTurretView turretView;
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
            turretView.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (targetRepository.PriorityTarget != null && mechWeapons.Armed)
        {
            turretView.LookAt(mechWeapons.GetPriorityDirection());
            Debug.Log("LookAt PriorityDirection");
        }
        else if (targetRepository.PriorityTarget == null && pathfinder.reachedEndOfPath)
        {
            turretView.LookAt(lookDirection);
            Debug.Log("LookAt LookDirection");
        }
    }
}
