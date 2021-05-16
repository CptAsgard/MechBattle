using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSpawner : NetworkBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField]
    private GameObject inputObject;

    public override void OnStartAuthority()
    {
        inputActions.Enable();
        CreateAuthoritativeInputObject();
    }

    [Command]
    private void CreateAuthoritativeInputObject()
    { 
        GameObject go = Instantiate(inputObject, transform);
        NetworkServer.Spawn(go, connectionToClient);
    }
}
