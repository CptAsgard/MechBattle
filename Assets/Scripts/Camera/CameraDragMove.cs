using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDragMove : MonoBehaviour
{
    [SerializeField]
    private InputActionReference mousePosition;
    [SerializeField]
    private bool resetPositionOnEndDrag = true;
    [SerializeField]
    private LayerMask groundLayerMask;

    private Vector3 worldStartPosition;
    private Vector3 cameraStartPosition;
    private bool isDragging = false;
    private Vector3 cameraPreviousPosition;

    public void Update()
    {
        if (!isDragging)
        {
            return;
        }            
        
        Vector3 screenPosition = mousePosition.action.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane, groundLayerMask))
        {
            //current raycast not hitting anything then don't move camera
            bool offscreen = screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > Camera.main.pixelWidth || screenPosition.y > Camera.main.pixelHeight;

            if (!offscreen)
            {
                //but if not hitting anything and mouse is onscreen, then assume player dragged to the void and reset camera position
                Camera.main.transform.position = cameraStartPosition;
            }

            return;
        }

        Vector3 worldDelta = worldStartPosition - ray.GetPoint(hit.distance);
        Camera.main.transform.position += worldDelta;

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

    public void OnDragAction(InputAction.CallbackContext callbackContext)
    {
        Vector3 screenPosition = mousePosition.action.ReadValue<Vector2>();
        if (callbackContext.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
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
}
