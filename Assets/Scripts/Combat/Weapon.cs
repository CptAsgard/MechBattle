using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform origin;
    public Transform Origin => origin;

    public Vector3 AimDirection;

    public abstract WeaponData WeaponData { get; }
    public abstract bool Armed { get; }

    public bool InRange { get; protected set; }
    protected MechData MechData { get; private set; }

    private void Start()
    {
        MechData = GetComponentInParent<MechData>();
    }

    public virtual void Fire()
    { }
}
