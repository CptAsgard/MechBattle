using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectorMask;

    public SelectionState selectionState = new SelectionState();

    private void Update()
    {
        return;
        if (!Camera.main) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, selectorMask))
            {
                selectionState.selected = hit.collider.GetComponentInParent<MechData>();
            }
            else
            {
                selectionState.selected = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && selectionState.selected != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}
