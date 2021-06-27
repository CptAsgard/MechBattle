using Mirror;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour
{
    [field: SyncVar]
    public MechState Owner { get; private set; }

    public Vector3 AimDirection = new Vector3();

    public abstract WeaponData WeaponData { get; }
    public abstract bool Armed { get; }

    public virtual void Initialize(MechState owner, Transform parent)
    {
        Owner = owner;
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;;
    }

    public virtual void Fire()
    { }
}
