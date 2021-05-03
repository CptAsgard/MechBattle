using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    public bool resetPositionOnEndDrag = true;
    public LayerMask groundLayerMask;

    private Vector3 worldStartPosition;
    private Vector3 cameraStartPosition;
    private Camera panCam;
    private bool isDragging = false;
    private Vector3 cameraPreviousPosition;

    private void Start()
    {
        panCam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!panCam)
        {
            Debug.LogError("No camera attached", gameObject);
            return;
        }

        RaycastHit hit;
        Vector3 screenPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = panCam.ScreenPointToRay(screenPosition);
            isDragging = Physics.Raycast(ray, out hit, panCam.farClipPlane, groundLayerMask);

            if (!isDragging)
            {
                return;
            }

            cameraStartPosition = panCam.transform.position;
            worldStartPosition = ray.GetPoint(hit.distance);
            return;
        }

        if (!isDragging)
        {
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Ray ray = panCam.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out hit, panCam.farClipPlane, groundLayerMask))
            {
                //current raycast not hitting anything then don't move camera
                bool offscreen = screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > panCam.pixelWidth || screenPosition.y > panCam.pixelHeight;

                if (!offscreen)
                {
                    //but if not hitting anything and mouse is onscreen, then assume player dragged to the void and reset camera position
                    panCam.transform.position = cameraStartPosition;
                }

                return;
            }

            Vector3 worldDelta = worldStartPosition - ray.GetPoint(hit.distance);
            panCam.transform.position += worldDelta;

            if (Physics.Raycast(panCam.transform.position, -Vector3.up, out RaycastHit downHit, panCam.farClipPlane, groundLayerMask))
            {
                panCam.transform.position = downHit.point + Vector3.up * 13f;
                cameraPreviousPosition = panCam.transform.position;
            }
            else
            {
                panCam.transform.position = cameraPreviousPosition;
            }
        }
        else
        {
            // reset and cleanup data from drag
            worldStartPosition = Vector3.zero;
            isDragging = false;

            if (resetPositionOnEndDrag)
            {
                panCam.transform.position = cameraStartPosition;
            }
        }
    }
}
