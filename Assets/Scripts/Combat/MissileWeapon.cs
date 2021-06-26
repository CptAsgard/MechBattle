using UnityEngine;

public class MissileWeapon : Weapon
{
    [SerializeField]
    private MissileWeaponData weaponData;
    [SerializeField]
    private WeaponReloadDelay reloadDelay;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => reloadDelay.ReadyToFire;
    public override bool ShouldAim => false;

    public override void Fire()
    {

    }
}
