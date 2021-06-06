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

    private List<Weapon> weapons = new List<Weapon>();

    public IEnumerable<Weapon> Weapons => weapons;
    public Transform WeaponsParent => weaponsParent;
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
    }

    public void Add(Weapon weapon)
    {
        weapon.transform.parent = weaponsParent;
        weapon.Initialize(mechState);
        weapons.Add(weapon);
    }

    public Vector2 GetPriorityDirection()
    {
        return weapons.First(weapon => weapon.Armed).AimDirection;
    }

    public void Aim(NetworkIdentity target)
    {
        mechState.Target = target;
    }
}
