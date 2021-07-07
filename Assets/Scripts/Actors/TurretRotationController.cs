using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class TurretRotationController : MonoBehaviour
{
    public float rotationSpeed;

    [SerializeField]
    private Transform targetTransform = null;
    [SerializeField]
    private float horizontalAngleLimit = 45f; // should never be 90d or turret will invert/snap at limits
    [SerializeField]
    private float verticalAngleLimit = 10f;
    
    public Vector3 Orientation => targetTransform.forward;
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
        if (targetTransform.forward == Target)
        {
            return;
        }

        float diffAngle = Vector3.Dot(targetTransform.forward, Target);
        if (diffAngle < 0.9999f || diffAngle > 1.0001f)
        {
            Quaternion from = Quaternion.LookRotation(targetTransform.forward);
            Quaternion to = Quaternion.LookRotation(Target);
            targetTransform.rotation = Quaternion.RotateTowards(from, to, rotationSpeed * Time.fixedDeltaTime * Mathf.Rad2Deg);
        }
        else
        {
            targetTransform.rotation = Quaternion.LookRotation(Target);
        }

        Quaternion clampedRotation = Quaternion.Euler(
            new Vector3(
                ClampAngle(targetTransform.localEulerAngles.x, -verticalAngleLimit, verticalAngleLimit),
                ClampAngle(targetTransform.localEulerAngles.y, -horizontalAngleLimit, horizontalAngleLimit),
                targetTransform.localEulerAngles.z));

        targetTransform.localRotation = clampedRotation;
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
