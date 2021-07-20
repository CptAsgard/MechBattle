using Pathfinding;
using UnityEngine;

public class MechTurretController : MonoBehaviour
{
    [SerializeField]
    private TurretRotationController turretRotation;
    [SerializeField]
    private MechWeaponsController mechWeapons;
    [SerializeField]
    private WeaponTargetRepository targetRepository;
    [SerializeField]
    private AIPath pathfinder;
    [SerializeField]
    private MechDataScriptableObject mechData;

    private Vector3 lookDirection;

    private void Start()
    {
        turretRotation.rotationSpeed = mechData.RotationSpeed;
    }

    private void Update()
    {
        if (targetRepository.TargetsCount != 0 && mechWeapons.Armed)
        {
            turretRotation.LookAt(mechWeapons.GetPriorityDirection());
        }
        else if (targetRepository.TargetsCount == 0 && pathfinder.reachedEndOfPath)
        {
            turretRotation.LookAt(lookDirection);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        lookDirection = direction;
        if (targetRepository.TargetsCount == 0 || !mechWeapons.Armed)
        {
            turretRotation.LookAt(lookDirection);
        }
    }
}
