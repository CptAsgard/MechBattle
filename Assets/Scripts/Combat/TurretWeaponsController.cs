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

    //[SerializeField]
    //private CombatTarget target;

    //private void Start()
    //{
    //    target = FindObjectOfType<CombatTarget>();
    //}

    //public void Tick()
    //{
    //    if (target.Current == null || weaponsPriority.All(weapon => !weapon.Armed) || weaponsPriority.All(weapon => !weapon.Armed))
    //    {
    //        return;
    //    }

    //    turretRotation.LookAt(GetPriorityDirection());
    //    FireWeapons();
    //}

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

            if (Mathf.Abs(Vector3.Angle(turretRotation.Orientation, weapon.AimDirection)) <= weapon.WeaponData.maxAngleDeviation)
            {
                weapon.Fire();
            }
        }
    }
}
