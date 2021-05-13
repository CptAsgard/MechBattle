using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeaponData", menuName = "MechBattle/ProjectileWeaponData", order = 1)]
public class ProjectileWeaponData : WeaponData
{
    public override WeaponType weaponType => WeaponType.Projectile;
    public float muzzleVelocity;
}
