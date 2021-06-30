using Mirror;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour
{
    [field: SyncVar]
    public MechState Owner { get; private set; }

    public Vector3 AimDirection = new Vector3();
    
    protected WeaponTargetRepository TargetRepository;

    public abstract WeaponData WeaponData { get; }

    public abstract bool Armed { get; }
    public abstract bool AutoAim { get; }

    private void FixedUpdate()
    {
        if (!isServer)
        {
            enabled = false;
            return;
        }

        if (!Owner)
        {
            return;
        }

        Aim();
    }

    public virtual void Initialize(MechState owner, Transform parent)
    {
        Owner = owner;
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        TargetRepository = owner.GetComponent<WeaponTargetRepository>();
    }

    public virtual void Fire()
    { }
    
    protected virtual void Aim()
    { }
}
