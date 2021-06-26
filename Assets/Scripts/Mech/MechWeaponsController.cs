using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class MechWeaponsController : NetworkBehaviour
{
    [SerializeField]
    private Transform weaponsParent;
    [SerializeField]
    private MechState mechState;

    private readonly List<Weapon> weapons = new List<Weapon>();

    public IEnumerable<Weapon> Weapons => weapons;
    public Transform WeaponsParent => weaponsParent;
    public bool Armed => weapons.Any(weapon => weapon.Armed);

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

    public void Add(Weapon weapon)
    {
        weapon.transform.parent = weaponsParent;
        weapon.Initialize(mechState);
        weapons.Add(weapon);
    }

    public Vector3 GetPriorityDirection()
    {
        return weapons.First(weapon => weapon.Armed).AimDirection;
    }
}
