using Pathfinding;
using UnityEngine;

public class MechTargetingController : MonoBehaviour
{
    [SerializeField]
    private MechTurretAngleController rotator;
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
            rotator.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (mechState.Target != null && mechWeapons.Armed)
        {
            rotator.LookAt(mechWeapons.GetPriorityDirection());
        }
        else if (mechState.Target == null && pathfinder.reachedEndOfPath)
        {
            rotator.LookAt(lookDirection);
        }
    }
}
