using Mirror;
using UnityEngine;

public class ProjectileWeaponView : NetworkBehaviour
{
    [SerializeField]
    private ProjectileWeapon weapon;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private TurretRotationController turretRotation;

    private void Start()
    {
        transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (weapon.Armed)
        {
            turretRotation.LookAt(weapon.AimDirection);
        }
    }
}
