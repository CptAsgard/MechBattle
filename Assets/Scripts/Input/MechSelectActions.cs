using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking.Types;

public class MechSelectActions : NetworkBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;
    [SerializeField]
    private LayerMask selectorMask;

    public SelectionState SelectionState = new SelectionState();

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.started)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        SelectionState.selected = Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask) ? hit.collider.GetComponentInParent<MechState>() : null;
    }

    public void TargetEnemy(InputAction.CallbackContext callbackContext)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask))
        {
            int selectedTeam = SelectionState.selected.Owner; // TODO check if owners on same team
            int targetTeam = hit.collider.GetComponentInParent<MechState>().Owner;

            if (selectedTeam != targetTeam)
            {
                SetTarget(SelectionState.selected.gameObject, hit.collider.transform.root.gameObject);
            }
        }
    }

    [Command]
    private void SetTarget(GameObject from, GameObject target, NetworkConnectionToClient sender = null)
    {
        from.GetComponentInParent<WeaponsController>().Aim(target.GetComponent<NetworkIdentity>());
    }
}
