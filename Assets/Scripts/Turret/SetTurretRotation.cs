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
    [SerializeField]
    private float horizontalAngleLimit = 45f; // should never be 90d or turret will invert/snap at limits
    [SerializeField]
    private float verticalAngleLimit = 10f;

    private Vector3 forward = Vector3.forward + Vector3.right;

    public Vector3 Orientation => turretTransform.forward;

    public void LookAt(Vector3 to)
    {
        forward = to;
    }

    private void Update()
    {
        UpdateTurretDirection();

        if (!ai.reachedEndOfPath && ai.hasPath)
        {
            return;
        }

        UpdateLegsDirection();
    }

    private void UpdateTurretDirection()
    {
        Vector3 lookDir = Vector3.RotateTowards(turretTransform.forward, forward, rotationSpeed * Time.deltaTime, 0f);

        turretTransform.rotation = Quaternion.LookRotation(lookDir); // assigning is expensive, we need localEulerAngles a different way

        Quaternion clampedRotation = Quaternion.Euler(
            new Vector3(
                ClampAngle(turretTransform.localEulerAngles.x, -verticalAngleLimit, verticalAngleLimit), 
                ClampAngle(turretTransform.localEulerAngles.y, -horizontalAngleLimit, horizontalAngleLimit), 
                turretTransform.localEulerAngles.z) 
            + transform.eulerAngles);

        turretTransform.rotation = clampedRotation;
    }

    private void UpdateLegsDirection()
    {
        Vector3 feetDir = Vector3.RotateTowards(transform.forward, forward, rotationSpeed / 2 * Time.deltaTime, 0f);
        feetDir.y = 0;

        Quaternion feetRotation = Quaternion.LookRotation(feetDir, Vector3.up);
        transform.rotation = feetRotation;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 90f || angle > 270f)
        {
            if (angle > 180f) angle -= 360f;
            if (max > 180f) max -= 360f;
            if (min > 180f) min -= 360f;
        }
        angle = Mathf.Clamp(angle, min, max);
        if (angle < 0f) angle += 360f;
        return angle;
    }
}
