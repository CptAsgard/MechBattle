using Mirror;
using UnityEngine;

public class ProjectileWeaponView : NetworkBehaviour
{
    [SerializeField]
    private ProjectileWeapon weapon;
    [SerializeField]
    private GameObject bulletPrefab;

    [ClientRpc]
    public void RpcFire(Vector3 position, Vector3 forward)
    {
        GameObject pr = Instantiate(bulletPrefab);
        pr.GetComponent<Projectile>().Initialize(position, forward, (ProjectileWeaponData) weapon.WeaponData);
        Debug.Log("RpcFire");
    }
}
