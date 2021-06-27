using Mirror;
using UnityEngine;

public class ProjectileWeaponView : NetworkBehaviour
{
    [SerializeField]
    private ProjectileWeapon weapon;
    [SerializeField]
    private GameObject bulletPrefab;

    private TurretRotationController turretRotation;

    private void Start()
    {
        turretRotation = transform.parent.GetComponent<TurretRotationController>();
    }

    private void FixedUpdate()
    {
        if (weapon.Armed)
        {
            turretRotation.LookAt(weapon.AimDirection);
        }
    }

    [ClientRpc]
    public void RpcFire(Vector3 position, Vector3 forward)
    {
        GameObject pr = Instantiate(bulletPrefab);
        pr.GetComponent<Projectile>().Initialize(position, forward, (ProjectileWeaponData) weapon.WeaponData);
    }
}
