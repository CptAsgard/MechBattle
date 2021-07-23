using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechSelectActions : NetworkBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;
    [SerializeField]
    private LayerMask selectorMask;

    public MechSelectionState MechSelectionState = new MechSelectionState();

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.started)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        MechSelectionState.selected = Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask) ? hit.collider.GetComponentInParent<MechState>() : null;
    }

    public void SelectMech(int mechIndex)
    {
        int playerIndex = NetworkClient.localPlayer.GetComponent<Player>().identity;
        var mechs = MechRepository.Instance.GetFriendly(playerIndex).ToList();
        MechSelectionState.selected = mechs[mechIndex];
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
