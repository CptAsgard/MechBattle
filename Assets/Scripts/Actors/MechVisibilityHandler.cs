using System.Linq;
using Mirror;
using UnityEngine;

public class MechVisibilityHandler : MonoBehaviour
{
    [SerializeField]
    private Transform sensorsTransform;
    [SerializeField]
    private LayerMask blockingMask;
    [SerializeField]
    private MechWeaponsController weaponsController;

    public bool CanSee(NetworkIdentity identity)
    {
        if (IsOwnedWeapon(identity)) return true;

        Vector3 center = identity.transform.position + Vector3.up;
        return Test(sensorsTransform.position, center + Vector3.left) ||
                Test(sensorsTransform.position, center + Vector3.right) ||
                Test(sensorsTransform.position, center + Vector3.up) ||
                Test(sensorsTransform.position, center + Vector3.down);
    }

    private bool IsOwnedWeapon(NetworkIdentity identity)
    {
        return weaponsController.Weapons.Any(weapon => weapon.netIdentity == identity);
    }

    private bool Test(Vector3 a, Vector3 b)
    {
        return !Physics.Linecast(a, b, blockingMask);
    }
}
