using Mirror;
using UnityEngine;

public readonly struct WeaponOwner : System.IEquatable<WeaponOwner>
{
    public readonly MechState Owner;
    public readonly WeaponAttachmentPoint AttachmentPoint;

    public WeaponOwner(MechState owner, WeaponAttachmentPoint attachmentPoint)
    {
        Owner = owner;
        AttachmentPoint = attachmentPoint;
    }

    public bool Equals(WeaponOwner other)
    {
        return Owner == other.Owner && AttachmentPoint == other.AttachmentPoint;
    }
}

public abstract class WeaponController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnOwnerChanged))]
    public WeaponOwner WeaponOwner;

    public Vector3 AimDirection = new Vector3();

    protected WeaponTargetRepository targetRepository;
    protected TimeSince reloadTime;

    public abstract bool Armed { get; }
    public abstract WeaponData WeaponData { get; }
    public abstract bool AutoAim { get; }

    public override void OnStartClient()
    {
        if (isServer)
        {
            return;
        }
        
        enabled = false;
    }
    
    public virtual void Initialize(GameObject mech, WeaponAttachmentPoint attachmentPoint)
    {
        WeaponOwner = new WeaponOwner(mech.GetComponent<MechState>(), attachmentPoint);
        SetParent();
    }

    public virtual void Fire()
    { }

    protected virtual void Aim()
    { }

    private void OnOwnerChanged(WeaponOwner oldOwner, WeaponOwner newOwner)
    {
        if (!isServer)
        {
            SetParent();
        }
    }
    
    private void SetParent()
    {
        targetRepository = WeaponOwner.Owner.GetComponent<WeaponTargetRepository>();

        Transform parent = WeaponOwner.Owner.GetComponent<MechWeaponsController>().Attach(this, WeaponOwner.AttachmentPoint);
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
