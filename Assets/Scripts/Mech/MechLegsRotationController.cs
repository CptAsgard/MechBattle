using Pathfinding;
using UnityEngine;

public class MechLegsRotationController : MonoBehaviour
{
    [SerializeField]
    private AIPath ai = null;
    [SerializeField]
    private MechTurretRotationController turretRotation;
    [SerializeField]
    private MechDataScriptableObject mechData;

    private void FixedUpdate()
    {
        if (ai.hasPath && !ai.reachedEndOfPath)
        {
            return;
        }

        UpdateLegsDirection();
    }

    private void UpdateLegsDirection()
    {
        Vector3 feetDir = Vector3.RotateTowards(transform.forward, turretRotation.Target, mechData.RotationSpeed * Time.fixedDeltaTime, 0f);
        feetDir.y = 0;

        Quaternion feetRotation = Quaternion.LookRotation(feetDir, Vector3.up);
        transform.rotation = feetRotation;
    }
}
