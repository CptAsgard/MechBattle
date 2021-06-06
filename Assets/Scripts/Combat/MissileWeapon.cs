using UnityEngine;

public class MissileWeapon : Weapon
{
    [SerializeField]
    private MissileWeaponData weaponData;
    [SerializeField]
    private WeaponFireController fireController;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => fireController.ReadyToFire;
    public override bool ShouldAim => false;

    public override void Fire()
    {

    }
}
