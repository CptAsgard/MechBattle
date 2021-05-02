using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    public bool resetPositionOnEndDrag = true;
    public LayerMask groundLayerMask;

    private Vector3 worldStartPosition;
    private Vector3 cameraStartPosition;
    private Camera panCam;
    private bool isdragging = false;

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
            isdragging = Physics.Raycast(ray, out hit, panCam.farClipPlane, groundLayerMask);

            if (!isdragging)
            {
                return;
            }

            cameraStartPosition = panCam.transform.position;
            worldStartPosition = ray.GetPoint(hit.distance);
            return;
        }

        if (!isdragging)
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
        }
        else
        {
            // reset and cleanup data from drag
            worldStartPosition = Vector3.zero;
            isdragging = false;

            if (resetPositionOnEndDrag)
            {
                panCam.transform.position = cameraStartPosition;
            }
        }
    }
}
