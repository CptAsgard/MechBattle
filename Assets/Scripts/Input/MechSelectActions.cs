using UnityEngine;
using UnityEngine.InputSystem;

public class MechSelectActions : MonoBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;
    [SerializeField]
    private LayerMask selectorMask;

    public SelectionState selectionState = new SelectionState();

    public void Select(InputAction.CallbackContext callbackContext)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask))
        {
            selectionState.selected = hit.collider.GetComponentInParent<MechData>();
        }
        else
        {
            selectionState.selected = null;
        }
    }

    public void TargetEnemy(InputAction.CallbackContext callbackContext)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask))
        {
            MechData selectedData = selectionState.selected.GetComponent<MechData>();
            int selectedTeam = selectedData.team;
            int targetTeam = hit.collider.GetComponentInParent<MechData>().team;

            if (selectedTeam != targetTeam)
            {
                selectedData.target = hit.collider.transform.parent;
            }
        }
    }
}
