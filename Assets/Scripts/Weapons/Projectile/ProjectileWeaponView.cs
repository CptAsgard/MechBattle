using Mirror;
using UnityEngine;

public class ProjectileWeaponView : NetworkBehaviour
{
    [SerializeField]
    private ProjectileWeaponController weaponController;
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
        if (weaponController.Armed)
        {
            turretRotation.LookAt(weaponController.AimDirection);
        }
    }
}
