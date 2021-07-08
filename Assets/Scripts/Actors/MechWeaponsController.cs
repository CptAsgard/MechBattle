using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public enum WeaponAttachmentPoint
{
    Left,
    Right
}

public class MechWeaponsController : NetworkBehaviour
{
    [SerializeField]
    private Transform weaponsParentLeft;
    [SerializeField]
    private Transform weaponsParentRight;
    [SerializeField]
    private MechState mechState;

    private readonly List<WeaponController> weapons = new List<WeaponController>();

    public IEnumerable<WeaponController> Weapons => weapons;
    public bool Armed => weapons.Any(weapon => weapon.Armed);

    private Vector3 lastDirection;

    //private void FixedUpdate()
    //{
        //if (mechState.Target == null)
        //{
        //    return;
        //}

        //if (!mechState.Target.observers.ContainsValue(mechState.Owner) ||
        //    mechState.Target.GetComponent<MechState>().PowerState == MechPowerState.Destroyed)
        //{
        //    mechState.Target = null;
        //}

        // TODO : Sets target to null if target can't see us or if target is destroyed. Replicate in TargetController
    //}

    public Transform Add(WeaponController weaponController, WeaponAttachmentPoint attachmentPoint)
    {
        weapons.Add(weaponController);

        Transform parent = attachmentPoint == WeaponAttachmentPoint.Left ? weaponsParentLeft : weaponsParentRight;
        return parent;
    }
    
    public Vector3 GetPriorityDirection()
    {
        WeaponController firstWeapon = weapons.FirstOrDefault(weapon => weapon.AutoAim && weapon.Armed);
        if (firstWeapon != null)
        {
            lastDirection = firstWeapon.AimDirection;
        }
        return lastDirection;
    }
}
