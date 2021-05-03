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

        AIPath ai = selector.selectionState.selected.GetComponentInParent<AIPath>();

        if (Input.GetMouseButtonDown(1))
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit downHit, 100))
            {
                return;
            }

            Vector3 targetDirection = downHit.point - selector.selectionState.selected.transform.position;
            if (targetDirection.sqrMagnitude < 16 && Vector3.Angle(targetDirection, selector.selectionState.selected.transform.forward) > 140)
            {
                ai.enableRotation = false;
                ai.maxSpeed = 1.75f;
                ai.maxAcceleration = 5f;
            }
            else
            {
                ai.enableRotation = true;
                ai.maxSpeed = 3.5f;
                ai.maxAcceleration = 10f;
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
