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

    private readonly List<Weapon> weapons = new List<Weapon>();

    public IEnumerable<Weapon> Weapons => weapons;
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

    public Transform Add(Weapon weapon, WeaponAttachmentPoint attachmentPoint)
    {
        weapons.Add(weapon);

        Transform parent = attachmentPoint == WeaponAttachmentPoint.Left ? weaponsParentLeft : weaponsParentRight;
        return parent;
    }
    
    public Vector3 GetPriorityDirection()
    {
        Weapon firstWeapon = weapons.FirstOrDefault(weapon => weapon.AutoAim && weapon.Armed);
        if (firstWeapon != null)
        {
            lastDirection = firstWeapon.AimDirection;
        }
        return lastDirection;
    }
}
