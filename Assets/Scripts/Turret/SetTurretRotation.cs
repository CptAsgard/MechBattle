using Pathfinding;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SetTurretRotation : MonoBehaviour
{
    [SerializeField]
    private Transform turretTransform = null;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float angleLimit = 45f;

    private Vector3 target = Vector3.forward + Vector3.right;

    public void SetOrientation(Vector3 to)
    {
        target = to;
    }

    private void Update()
    {
        Vector3 lookDir = Vector3.RotateTowards(turretTransform.forward, target, rotationSpeed * Time.deltaTime, 0f);
        lookDir.y = turretTransform.forward.y; // height

        Quaternion newRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        turretTransform.rotation = newRotation;

        Quaternion clampedRotation = Quaternion.Euler(new Vector3(turretTransform.localEulerAngles.x, ClampAngle(turretTransform.localEulerAngles.y, -angleLimit, angleLimit), turretTransform.localEulerAngles.z) + transform.eulerAngles);
        turretTransform.rotation = clampedRotation;

        Debug.DrawRay(transform.position, target, Color.red);
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
