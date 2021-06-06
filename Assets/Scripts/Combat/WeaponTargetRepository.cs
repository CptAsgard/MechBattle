using Mirror;
using UnityEngine;

public class WeaponTargetRepository : MonoBehaviour
{
    public NetworkIdentity PriorityTarget { get; private set; }

    public void SetPriorityTarget(NetworkIdentity target)
    {
        PriorityTarget = target;
    }
}
