using Mirror;
using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechMoveActions : NetworkBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;

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

        if (MechSelectActions.MechSelectionState.selected == null || MechSelectActions.MechSelectionState.selected.PowerState != MechPowerState.PowerOn || MechSelectActions.MechSelectionState.selected.PlayerIndex != player.identity)
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

            WalkTowards(MechSelectActions.MechSelectionState.selected.gameObject, downHit.point);
        }

        if (callbackContext.canceled)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>()), out RaycastHit upHit, 100))
            {
                return;
            }

            LookTowards(MechSelectActions.MechSelectionState.selected.gameObject, upHit.point);
        }
    }

    [Command]
    private void WalkTowards(GameObject target, Vector3 point, NetworkConnectionToClient sender = null)
    {
        MechState mechState = target.GetComponent<MechState>();
        if (sender == null || mechState.PlayerIndex != player.identity || mechState.PowerState != MechPowerState.PowerOn)
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
        MechState mechState = target.GetComponent<MechState>();
        if (sender == null || mechState.PlayerIndex != player.identity || mechState.PowerState != MechPowerState.PowerOn)
        {
            return;
        }

        AIPath ai = target.GetComponentInChildren<AIPath>();

        Vector3 dirVector = point - (ai.hasPath ? ai.destination : target.transform.position);
        if (dirVector.sqrMagnitude < 1f) // TODO : double check function, maybe not hardcode
        {
            return;
        }

        MechTurretController turret = target.GetComponentInChildren<MechTurretController>();
        turret.SetDirection(dirVector);
    }
}
