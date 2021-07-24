using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechSelectActions : NetworkBehaviour
{
    public static MechSelectionState MechSelectionState = new MechSelectionState();

    [SerializeField]
    private InputActionReference mousePosition;
    [SerializeField]
    private LayerMask selectorMask;
    [SerializeField]
    private CameraMoveActions cameraMove;

    private int previousSelected;

    public void SetTargetMechIndex(int mechIndex)
    {
        MechSelectionState.TargetMechIndex = mechIndex;
    }

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.started)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
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

        if (previousSelected == MechSelectionState.TargetMechIndex)
        {
            cameraMove.FocusFriendlyMech(MechSelectionState.TargetMechIndex);
        }

        previousSelected = MechSelectionState.TargetMechIndex;
    }

    public void TargetEnemy(InputAction.CallbackContext callbackContext)
    {
        if (!MechSelectionState.selected || MechSelectionState.selected.PowerState == MechPowerState.Destroyed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
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
