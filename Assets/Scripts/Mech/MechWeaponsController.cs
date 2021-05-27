using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class MechWeaponsController : NetworkBehaviour
{
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private MechTurretAngleController mechTurretController;
    [SerializeField]
    private MechState mechState;

    public IEnumerable<Weapon> Weapons => weapons;

    private List<Weapon> weapons = new List<Weapon>();

    public bool Armed => weapons.Any(weapon => weapon.Armed);

    private void FixedUpdate()
    {
        if (mechState.Target == null)
        {
            return;
        }

        if (!mechState.Target.observers.ContainsValue(mechState.Owner) ||
            mechState.Target.GetComponent<MechState>().PowerState == MechPowerState.Destroyed)
        {
            mechState.Target = null;
        }

        Fire();
    }

    public void Add(Weapon weapon)
    {
        int slotIndex = -1;
        for (int i = 0; i < slotParent.childCount; i++)
        {
            if (slotParent.GetChild(i).childCount != 0)
            {
                continue;
            }

            slotIndex = i;
            break;
        }

        if (slotIndex == -1)
        {
            Debug.LogError($"ERROR: Tried to add weapon {weapon.name} to mech {name} but there is no free slot!", gameObject);
            return;
        }

        weapon.transform.parent = slotParent.GetChild(slotIndex);
        weapon.Initialize(slotIndex, mechState);
        weapons.Add(weapon);
    }

    public Transform GetSlot(int index)
    {
        return slotParent.GetChild(index);
    }

    public Vector3 GetPriorityDirection()
    {
        return weapons.First(weapon => weapon.Armed).AimDirection;
    }

    public void Aim(NetworkIdentity target)
    {
        mechState.Target = target;
    }

    public void Fire()
    {
        foreach (Weapon weapon in weapons)
        {
            if (!weapon.Armed || !weapon.InRange)
            {
                continue;
            }

            // TODO : Sketch to rely on actual angle in world to determine whether turret has moved in that direction, precision could be off
            if (Mathf.Abs(Vector3.Angle(mechTurretController.Orientation, weapon.AimDirection)) <= weapon.WeaponData.maxAngleDeviation)
            {
                weapon.Fire();
            }
        }
    }
}
