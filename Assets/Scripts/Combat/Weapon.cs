using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform origin;
    public Transform Origin => origin;

    public Vector3 AimDirection;

    public abstract WeaponData WeaponData { get; }
    public abstract bool Armed { get; }

    public bool WithinRange { get; protected set; }
    protected CombatTarget Target { get; private set; }

    private void Start()
    {
        Target = FindObjectOfType<CombatTarget>();
    }

    public virtual void Fire()
    { }
}
