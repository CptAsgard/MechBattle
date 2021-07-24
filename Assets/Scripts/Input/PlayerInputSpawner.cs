using Mirror;
using UnityEngine;

public class PlayerInputSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject inputObject;

    public override void OnStartAuthority()
    {
        CreateAuthoritativeInputObject();
    }

    [Command]
    private void CreateAuthoritativeInputObject()
    { 
        GameObject go = Instantiate(inputObject, transform);
        NetworkServer.Spawn(go, connectionToClient);
    }
}
