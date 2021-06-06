using Pathfinding;
using UnityEngine;

public class MechTurretController : MonoBehaviour
{
    [SerializeField]
    private MechTurretView turretView;
    [SerializeField]
    private MechWeaponsController mechWeapons;
    [SerializeField]
    private MechState mechState;
    [SerializeField]
    private AIPath pathfinder;

    private Vector3 lookDirection;

    public void SetDirection(Vector3 direction)
    {
        lookDirection = direction;
        if (mechState.Target == null || !mechWeapons.Armed)
        {
            turretView.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (mechState.Target != null && mechWeapons.Armed)
        {
            turretView.LookAt(mechWeapons.GetPriorityDirection());
        }
        else if (mechState.Target == null && pathfinder.reachedEndOfPath)
        {
            turretView.LookAt(lookDirection);
        }
    }
}
