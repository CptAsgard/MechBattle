using Mirror;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour
{
    [field: SyncVar]
    public MechState Owner { get; private set; }
    [field: SyncVar]
    public int Slot { get; private set; }
    public Transform Origin { get; private set; }
    public bool InRange { get; protected set; }
    
    public Vector3 AimDirection;

    public abstract WeaponData WeaponData { get; }
    public abstract bool Armed { get; }

    public void Initialize(int slot, MechState owner)
    {
        Slot = slot;
        Owner = owner;

        Origin = owner.GetComponent<WeaponsController>().GetSlot(slot);
    }

    public virtual void Fire()
    { }
}
