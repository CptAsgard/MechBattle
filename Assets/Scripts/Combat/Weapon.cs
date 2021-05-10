using Mirror;
using UnityEngine;

public abstract class Weapon : NetworkBehaviour
{
    [SerializeField]
    private WeaponData weaponData;
    public WeaponData WeaponData => weaponData;

    [SerializeField]
    private Transform origin;
    public Transform Origin => origin;

    [Command]
    public virtual void Fire()
    { }
}
