using Pathfinding;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    [SerializeField]
    private TurretRotation rotator;
    [SerializeField]
    private WeaponsController weapons;
    [SerializeField]
    private AIPath pathfinder;
    [SerializeField]
    private MechData data;

    private Vector3 lookDirection;

    public void SetDirection(Vector3 direction)
    {
        lookDirection = direction;
        if (data.target == null || !weapons.Armed)
        {
            rotator.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (data.target != null && weapons.Armed)
        {
            rotator.LookAt(weapons.GetPriorityDirection());
            weapons.FireWeapons(); // TODO : yeah no, not the firing controller
        }
        else if (data.target == null && pathfinder.reachedEndOfPath)
        {
            rotator.LookAt(lookDirection);
        }
    }
}
