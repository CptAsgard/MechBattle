using Pathfinding;
using UnityEngine;

public class TargetingController : MonoBehaviour
{
    [SerializeField]
    private SetTurretRotation rotator;
    [SerializeField]
    private TurretWeaponsController weapons;
    [SerializeField]
    private AIPath pathfinder;

    private CombatTarget target;
    private Vector3 lookDirection;

    private void Start()
    {
        target = FindObjectOfType<CombatTarget>();
    }

    public void SetDirection(Vector3 direction)
    {
        lookDirection = direction;
        if (target.Current == null || !weapons.Armed)
        {
            rotator.LookAt(lookDirection);
        }
    }

    private void Update()
    {
        if (target.Current != null && weapons.Armed)
        {
            rotator.LookAt(weapons.GetPriorityDirection());
            weapons.FireWeapons();
        }
        else if (target.Current == null && pathfinder.reachedEndOfPath)
        {
            rotator.LookAt(lookDirection);
        }
    }
}
