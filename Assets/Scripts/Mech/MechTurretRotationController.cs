using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MechTurretRotationController : MonoBehaviour
{
    [SerializeField]
    private Transform turretTransform = null;
    [SerializeField]
    private float horizontalAngleLimit = 45f; // should never be 90d or turret will invert/snap at limits
    [SerializeField]
    private float verticalAngleLimit = 10f;
    [SerializeField]
    private MechDataScriptableObject mechData;

    public Vector3 Orientation => turretTransform.forward;
    public Vector3 Target { get; private set; } = Vector3.forward;

    public void LookAt(Vector3 forward)
    {
        Target = forward;
    }

    private void FixedUpdate()
    {
        UpdateTurretDirection();
    }

    private void UpdateTurretDirection()
    {
        if (turretTransform.forward == Target)
        {
            return;
        }

        float diffAngle = Vector3.Dot(turretTransform.forward, Target);
        if (diffAngle < 0.9999f || diffAngle > 1.0001f)
        {
            Vector3 lookDir = Vector3.RotateTowards(turretTransform.forward, Target, mechData.RotationSpeed * Time.fixedDeltaTime, 0f);

            turretTransform.rotation = Quaternion.LookRotation(lookDir);

            Quaternion clampedRotation = Quaternion.Euler(
                new Vector3(
                    ClampAngle(turretTransform.localEulerAngles.x, -verticalAngleLimit, verticalAngleLimit),
                    ClampAngle(turretTransform.localEulerAngles.y, -horizontalAngleLimit, horizontalAngleLimit),
                    turretTransform.localEulerAngles.z)
                + transform.eulerAngles);

            turretTransform.rotation = clampedRotation;
        }
        else
        {
            turretTransform.rotation = Quaternion.LookRotation(Target);
        }
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
