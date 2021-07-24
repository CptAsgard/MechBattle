using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoveActions : NetworkBehaviour
{
    [SerializeField]
    private bool resetPositionOnEndDrag = true;
    [SerializeField]
    private LayerMask groundLayerMask;

    private Vector3 worldStartPosition;
    private Vector3 cameraStartPosition;
    private bool isDragging = false;
    private Vector3 cameraPreviousPosition;
    private Vector2 mousePosition;

    public override void OnStartClient()
    {
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.FindAction("MousePosition").performed += context => mousePosition = context.ReadValue<Vector2>();
        actionMap.FindAction("Drag").started += OnDragAction;
        actionMap.FindAction("Drag").performed += OnDragAction;
        actionMap.FindAction("Drag").canceled += OnDragAction;

        FocusFriendlyMech(0);
    }

    private void Update()
    {
        if (!isDragging)
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane, groundLayerMask))
        {
            //current raycast not hitting anything then don't move camera
            bool offscreen = mousePosition.x < 0 || mousePosition.y < 0 ||
                mousePosition.x > Camera.main.pixelWidth || mousePosition.y > Camera.main.pixelHeight;

            if (!offscreen)
            {
                //but if not hitting anything and mouse is onscreen, then assume player dragged to the void and reset camera position
                Camera.main.transform.position = cameraStartPosition;
            }

            return;
        }

        Vector3 worldDelta = worldStartPosition - ray.GetPoint(hit.distance);
        Camera.main.transform.position += worldDelta;

        UpdateCameraHeight();
    }

    public void OnDragAction(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            isDragging = Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane, groundLayerMask);

            if (!isDragging)
            {
                return;
            }

            cameraStartPosition = Camera.main.transform.position;
            worldStartPosition = ray.GetPoint(hit.distance);
            return;
        }

        if (!callbackContext.performed)
        {
            // reset and cleanup data from drag
            worldStartPosition = Vector3.zero;
            isDragging = false;

            if (resetPositionOnEndDrag)
            {
                Camera.main.transform.position = cameraStartPosition;
            }
        }
    }
    
    public void FocusFriendlyMech(int mechIndex)
    {
        int playerIndex = NetworkClient.localPlayer.GetComponent<Player>().identity;
        var mechs = MechRepository.Instance.GetFriendly(playerIndex).ToList();
        
        Camera.main.transform.position = mechs[mechIndex].transform.position + Vector3.up * 13f;
        UpdateCameraHeight();
    }

    private void UpdateCameraHeight()
    {
        if (Physics.Raycast(Camera.main.transform.position, -Vector3.up, out RaycastHit downHit, Camera.main.farClipPlane, groundLayerMask))
        {
            Camera.main.transform.position = downHit.point + Vector3.up * 13f;
            cameraPreviousPosition = Camera.main.transform.position;
        }
        else
        {
            Camera.main.transform.position = cameraPreviousPosition;
        }
    }
}
