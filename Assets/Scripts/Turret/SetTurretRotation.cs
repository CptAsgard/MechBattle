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

    private Vector3 target = Vector3.forward + Vector3.right;

    public void SetOrientation(Vector3 to)
    {
        target = to;
    }

    private void Update()
    {
        Vector3 newTurretDir = Vector3.RotateTowards(turretTransform.forward, target, rotationSpeed * Time.deltaTime, 0f);
        newTurretDir.y = turretTransform.forward.y;
        turretTransform.rotation = Quaternion.LookRotation(newTurretDir, Vector3.up);
    }
}
