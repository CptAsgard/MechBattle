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

    [SyncVar]
    private WeaponAttachmentPoint attachmentPoint;

    public override void OnStartClient()
    {
        if (!isServer)
        {
            SetParent();
        }
    }
    
    public override void OnStartServer()
    {
        SetParent();
    }
    
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
    
    public virtual void Initialize(GameObject mech, WeaponAttachmentPoint attachmentPoint)
    {
        Owner = mech.GetComponent<MechState>();
        this.attachmentPoint = attachmentPoint;
    }

    public virtual void Fire()
    { }
    
    protected virtual void Aim()
    { }
    
    private void SetParent()
    {
        TargetRepository = Owner.GetComponent<WeaponTargetRepository>();

        Transform parent = Owner.GetComponent<MechWeaponsController>().Add(this, attachmentPoint);
        
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
