using Mirror;
using UnityEngine;

public class MechData : NetworkBehaviour
{
    [SyncVar]
    public int owner;
    public int team = 0;
    public Transform target;
}
