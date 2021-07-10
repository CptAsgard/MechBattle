using Mirror;
using UnityEngine;

public abstract class WeaponController : NetworkBehaviour
{
    [field: SyncVar]
    public MechState Owner { get; private set; }

    public Vector3 AimDirection = new Vector3();

    protected WeaponTargetRepository targetRepository;
    protected TimeSince reloadTime;

    public abstract bool Armed { get; }
    public abstract WeaponData WeaponData { get; }
    public abstract bool AutoAim { get; }

    [SyncVar]
    private WeaponAttachmentPoint attachmentPoint;

    public override void OnStartClient()
    {
        if (isServer)
        {
            return;
        }

        SetParent();
        enabled = false;
    }

    public override void OnStartServer()
    {
        SetParent();
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
        targetRepository = Owner.GetComponent<WeaponTargetRepository>();

        Transform parent = Owner.GetComponent<MechWeaponsController>().Add(this, attachmentPoint);

        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
