using System.Numerics;
using Pathfinding;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SetTurretRotation : MonoBehaviour
{
    [SerializeField]
    private AIPath ai = null;
    [SerializeField]
    private Transform turretTransform = null;
    [SerializeField]
    private float rotationSpeed = 1f;

    private Vector3 target = Vector3.forward;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                return;
            }

            Vector3 dirVector = hit.point - ai.destination;
            if (dirVector.sqrMagnitude > 1f)
            {
                target = dirVector;
            }
        }

        Vector3 newTurretDir = Vector3.RotateTowards(turretTransform.forward, target, rotationSpeed * Time.deltaTime, 0f);
        newTurretDir.y = turretTransform.forward.y;
        turretTransform.rotation = Quaternion.LookRotation(newTurretDir, Vector3.up);
    }
}
