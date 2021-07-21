using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class MechVisibilityHandler : NetworkBehaviour
{
    [SerializeField]
    private Transform sensorsTransform;
    [SerializeField]
    private LayerMask blockingMask;
    [SerializeField]
    private MechWeaponsController weaponsController;

    private List<NetworkIdentity> visibleIdentities = new List<NetworkIdentity>();

    public IEnumerable<NetworkIdentity> Visible => visibleIdentities;

    private void FixedUpdate()
    {
        visibleIdentities.Clear();
        foreach (NetworkIdentity identity in FindObjectsOfType<NetworkIdentity>().Where(id => id.gameObject.layer == LayerMask.NameToLayer("Player") && 
            id.gameObject.GetComponent<MechState>()?.PlayerIndex != GetComponent<MechState>().PlayerIndex))
        {
            if (CanSee(identity))
            {
                visibleIdentities.Add(identity);
            }
        }
    }

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
        Debug.DrawRay(a, b - a, Color.red);
        return !Physics.Linecast(a, b, blockingMask);
    }
}
