using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechSelectActions : NetworkBehaviour
{
    public static MechSelectionState MechSelectionState = new MechSelectionState();
    
    [SerializeField]
    private CameraMoveActions cameraMove;
    [SerializeField]
    private LayerMask selectorMask;

    private int previousSelected = -1;
    private Vector2 mousePosition;

    private void Start()
    {
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.FindAction("MousePosition").performed += context => mousePosition = context.ReadValue<Vector2>();
        actionMap.FindAction("Select").performed += Select;
        actionMap.FindAction("Focus Mech").performed += FocusFriendlyMech;
        actionMap.FindAction("Command").performed += TargetEnemy;
        actionMap.FindAction("Select Mech 1").performed += ctx => SetTargetMechIndex(ctx, 0);
        actionMap.FindAction("Select Mech 2").performed += ctx => SetTargetMechIndex(ctx, 1);
        actionMap.FindAction("Select Mech 3").performed += ctx => SetTargetMechIndex(ctx, 2);
        actionMap.FindAction("Select Mech 4").performed += ctx => SetTargetMechIndex(ctx, 3);
    }

    public void SetTargetMechIndex(InputAction.CallbackContext callbackContext, int mechIndex)
    {
        MechSelectionState.TargetMechIndex = mechIndex;

        SelectFriendlyMech(callbackContext);
    }

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        MechSelectionState.selected = Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask) ? hit.collider.GetComponentInParent<MechState>() : null;
    }

    public void SelectFriendlyMech(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed)
        {
            return;
        }

        int playerIndex = NetworkClient.localPlayer.GetComponent<Player>().identity;
        var mechs = MechRepository.Instance.GetFriendly(playerIndex).ToList();

        MechSelectionState.selected = mechs[MechSelectionState.TargetMechIndex];
    }

    public void FocusFriendlyMech(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed)
        {
            return;
        }

        if (MechSelectionState.selected == null)
        {
            return;
        }

        cameraMove.FocusFriendlyMech(MechSelectionState.TargetMechIndex);
    }

    public void TargetEnemy(InputAction.CallbackContext callbackContext)
    {
        if (!MechSelectionState.selected || MechSelectionState.selected.PowerState == MechPowerState.Destroyed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask))
        {
            int selectedTeam = MechSelectionState.selected.PlayerIndex; // TODO check if owners on same team
            MechState targetState = hit.collider.GetComponentInParent<MechState>();
            int targetTeam = targetState.PlayerIndex;

            if (targetState.PowerState != MechPowerState.Destroyed && selectedTeam != targetTeam)
            {
                SetTarget(MechSelectionState.selected.gameObject, hit.collider.transform.root.gameObject);
            }
        }
    }

    [Command]
    private void SetTarget(GameObject from, GameObject target, NetworkConnectionToClient sender = null)
    {
        from.GetComponentInParent<WeaponTargetRepository>().SetPriorityTarget(target.GetComponent<NetworkIdentity>());
    }
}
