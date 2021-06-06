using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeaponData", menuName = "MechBattle/ProjectileWeaponData", order = 1)]
public class ProjectileWeaponData : WeaponData
{
    public float muzzleVelocity;
    public float damageOnHit;
}
