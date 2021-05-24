using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class WeaponsController : NetworkBehaviour
{
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private TurretRotationController turretController;
    [SerializeField]
    private MechState mechState;

    private List<Weapon> weapons = new List<Weapon>();

    public bool Armed => weapons.Any(weapon => weapon.Armed);

    private void FixedUpdate()
    {
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

    public void Aim(Transform target)
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
            if (Mathf.Abs(Vector3.Angle(turretController.Orientation, weapon.AimDirection)) <= weapon.WeaponData.maxAngleDeviation)
            {
                weapon.Fire();
            }
        }
    }
}
