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

    public void TargetEnemy(InputAction.CallbackContext callbackContext)
    {
        if (!MechSelectionState.selected || MechSelectionState.selected.PowerState == MechPowerState.Destroyed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask))
        {
            int selectedTeam = MechSelectionState.selected.Owner; // TODO check if owners on same team
            int targetTeam = hit.collider.GetComponentInParent<MechState>().Owner;

            if (selectedTeam != targetTeam)
            {
                SetTarget(MechSelectionState.selected.gameObject, hit.collider.transform.root.gameObject);
            }
        }
    }

    [Command]
    private void SetTarget(GameObject from, GameObject target, NetworkConnectionToClient sender = null)
    {
        from.GetComponentInParent<MechWeaponsController>().Aim(target.GetComponent<NetworkIdentity>());
    }
}
