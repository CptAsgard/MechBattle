using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretWeaponsController : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weaponsPriority;
    [SerializeField]
    private SetTurretRotation turretRotation;
    [SerializeField]
    private CombatTarget target;
    
    private void Start()
    {
        target = FindObjectOfType<CombatTarget>();
    }

    private void Update()
    {
        if (target.Current == null || weaponsPriority.All(weapon => !weapon.WithinRange) || weaponsPriority.All(weapon => !weapon.Armed))
        {
            return;
        }

        turretRotation.LookAt(GetPriorityDirection());
        FireWeapons();
    }

    public Vector3 GetPriorityDirection()
    {
        return weaponsPriority.First(weapon => weapon.Armed).AimDirection;
    }

    // TODO : this is going to be a command probably
    private void FireWeapons()
    {
        foreach (Weapon weapon in weaponsPriority)
        {
            if (!weapon.Armed)
            {
                continue;
            }

            if (Mathf.Abs(Vector3.Angle(turretRotation.Orientation, weapon.AimDirection)) <= weapon.WeaponData.maxAngleDeviation)
            {
                weapon.Fire();
            }
        }
    }
}
