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

        IAstarAI ai = selector.selectionState.selected.GetComponentInParent<IAstarAI>();

        if (Input.GetMouseButtonDown(1))
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit downHit, 100))
            {
                return;
            }

            ai.destination = downHit.point;
        }

        if (!Input.GetMouseButtonUp(1))
        {
            return;
        }

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit upHit, 100))
        {
            return;
        }

        Vector3 dirVector = upHit.point - ai.destination;
        if (dirVector.sqrMagnitude < 1f)
        {
            return;
        }

        SetTurretRotation turret = selector.selectionState.selected.GetComponentInParent<SetTurretRotation>();
        turret.SetOrientation(dirVector);
    }
}
