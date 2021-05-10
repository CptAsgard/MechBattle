using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField]
    private GameObject projectile;

    public override void Fire()
    {
        GameObject bullet = Instantiate(projectile);
        bullet.GetComponent<Projectile>().Initialize(Origin.position, Origin.forward * WeaponData.projectileSpeed);
    }
}
