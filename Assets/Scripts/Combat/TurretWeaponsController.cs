using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretWeaponsController : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weaponsPriority;
    [SerializeField]
    private SetTurretRotation turretRotation;

    public bool Armed => weaponsPriority.Any(weapon => weapon.Armed);
    
    public Vector3 GetPriorityDirection()
    {
        return weaponsPriority.First(weapon => weapon.Armed).AimDirection;
    }

    // TODO : this is going to be a command probably
    public void FireWeapons()
    {
        foreach (Weapon weapon in weaponsPriority)
        {
            if (!weapon.Armed || !weapon.InRange)
            {
                continue;
            }

            // TODO : Sketch to rely on actual angle in world to determine whether turret has moved in that direction, precision could be off
            if (Mathf.Abs(Vector3.Angle(turretRotation.Orientation, weapon.AimDirection)) <= weapon.WeaponData.maxAngleDeviation)
            {
                weapon.Fire();
            }
        }
    }
}
