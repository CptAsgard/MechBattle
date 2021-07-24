using System.Linq;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoveActions : NetworkBehaviour
{
    [SerializeField]
    private float scrollSpeed;

    private float rotateInput;
    private Vector2 moveInput;

    public override void OnStartClient()
    {
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.FindAction("TurnCamera").performed += context => rotateInput = context.ReadValue<float>();
        actionMap.FindAction("MoveCamera").performed += context => moveInput = context.ReadValue<Vector2>();
        actionMap.FindAction("MoveCamera").canceled += context => moveInput = Vector2.zero;

        FocusFriendlyMech(0);
    }
 
    public float GetAxisCustom(string axisName)
    {
        return rotateInput;
    }

    private void Update()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;

        FocusTarget.Instance.transform.localRotation = Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0);
        FocusTarget.Instance.transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * scrollSpeed * Time.deltaTime, Space.Self);
    }
    
    public void FocusFriendlyMech(int mechIndex)
    {
        int playerIndex = NetworkClient.localPlayer.GetComponent<Player>().identity;
        var mechs = MechRepository.Instance.GetFriendly(playerIndex).ToList();

        FocusTarget.Instance.transform.position = mechs[mechIndex].transform.position;
    }
}
