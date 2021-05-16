using Mirror;
using Pathfinding;
using UnityEngine;

public class MoveToPoint : NetworkBehaviour
{
    [SerializeField]
    private Selector selector;
    [SerializeField]
    private Player player;

    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        if (selector.selectionState.selected == null || selector.selectionState.selected.owner != player.identity)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit downHit, 100))
            {
                return;
            }

            if (downHit.transform.CompareTag("Player"))
            {
                return;
            }

            WalkTowards(selector.selectionState.selected.gameObject, downHit.point);
        }

        if (!Input.GetMouseButtonUp(1))
        {
            return;
        }

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit upHit, 100))
        {
            return;
        }

        LookTowards(selector.selectionState.selected.gameObject, upHit.point);
    }

    [Command]
    private void WalkTowards(GameObject target, Vector3 point, NetworkConnectionToClient sender = null)
    {
        MechData mechData = target.GetComponent<MechData>();
        if (sender == null || mechData.owner != player.identity)
        {
            return;
        }

        AIPath ai = target.GetComponentInChildren<AIPath>();
        Vector3 targetDirection = point - target.transform.position;
        if (targetDirection.sqrMagnitude < 16 && Vector3.Angle(targetDirection, target.transform.forward) > 140)
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

        ai.destination = point;
    }

    [Command]
    private void LookTowards(GameObject target, Vector3 point, NetworkConnectionToClient sender = null)
    {        
        MechData mechData = target.GetComponent<MechData>();
        if (sender == null || mechData.owner != player.identity)
        {
            return;
        }

        AIPath ai = target.GetComponentInChildren<AIPath>();
        Vector3 dirVector = point - ai.destination;
        if (dirVector.sqrMagnitude < 1f)
        {
            return;
        }

        TargetingController turret = target.GetComponentInChildren<TargetingController>();
        turret.SetDirection(dirVector);
    }
}
