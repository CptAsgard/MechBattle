using Mirror;
using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechMoveActions : NetworkBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;
    [SerializeField]
    private MechSelectActions mechSelectActions;

    private Player player;

    private void Start()
    {
        player = isServer ? GetComponentInParent<Player>() : NetworkClient.localPlayer.GetComponent<Player>();
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        if (!hasAuthority)
        {
            return;
        }

        if (mechSelectActions.selectionState.selected == null || mechSelectActions.selectionState.selected.owner != player.identity)
        {
            return;
        }

        if (callbackContext.started)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>()), out RaycastHit downHit, 100))
            {
                return;
            }

            if (downHit.transform.CompareTag("Player"))
            {
                return;
            }

            WalkTowards(mechSelectActions.selectionState.selected.gameObject, downHit.point);
        }

        if (callbackContext.canceled)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>()), out RaycastHit upHit, 100))
            {
                return;
            }

            LookTowards(mechSelectActions.selectionState.selected.gameObject, upHit.point);
        }
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
