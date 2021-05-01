using Pathfinding;
using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    [SerializeField]
    private Selector selector;

    private void Update()
    {
        if (selector.selectionState.selected == null)
        {
            return;
        }

        if (!Input.GetMouseButtonDown(1))
        {
            return;
        }

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
        {
            return;
        }

        IAstarAI ai = selector.selectionState.selected.GetComponentInParent<IAstarAI>();
        ai.destination = hit.point;
    }
}
