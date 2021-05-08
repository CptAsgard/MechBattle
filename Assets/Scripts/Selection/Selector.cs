using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectorMask;

    public SelectionState selectionState = new SelectionState();

    private void Update()
    {
        if (!Camera.main) return;
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            selectionState.selected = hit.collider.GetComponentInParent<Selectable>();
        }
        else
        {
            selectionState.selected = null;
        }
    }
}
