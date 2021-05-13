using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform origin;
    public Transform Origin => origin;

    public Vector3 AimDirection;

    public abstract WeaponData WeaponData { get; }
    public abstract bool Armed { get; }

    protected CombatTarget target { get; private set; }

    private void Start()
    {
        target = FindObjectOfType<CombatTarget>();
    }

    public virtual void Fire()
    { }
}
