using Pathfinding;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    [SerializeField]
    private TurretRotationController rotator;
    [SerializeField]
    private WeaponsController weapons;
    [SerializeField]
    private AIPath pathfinder;
    [SerializeField]
    private MechState mechState;

    private Vector3 lookDirection;

    public void SetDirection(Vector3 direction)
    {
        lookDirection = direction;
        if (mechState.Target == null || !weapons.Armed)
        {
            rotator.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (mechState.Target != null && weapons.Armed)
        {
            rotator.LookAt(weapons.GetPriorityDirection());
        }
        else if (mechState.Target == null && pathfinder.reachedEndOfPath)
        {
            rotator.LookAt(lookDirection);
        }
    }
}
