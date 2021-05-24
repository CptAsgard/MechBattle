using Mirror;
using UnityEngine;

public class MechState : NetworkBehaviour
{
    [field: SyncVar]
    public int Owner { get; private set; }
    [field: SyncVar]
    public NetworkIdentity Target { get; set; }

    public void Initialize(int owner)
    {
        Owner = owner;
    }
}
