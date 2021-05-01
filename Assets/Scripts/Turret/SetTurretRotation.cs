using UnityEngine;
using UnityEngine.AI;

public class SetTurretRotation : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent = null;
    [SerializeField]
    private Transform bodyTransform = null;
    [SerializeField]
    private Transform turretTransform = null;
    [SerializeField]
    private float rotationSpeed = 1f;

    private void Start()
    {
        agent.updateRotation = false;
    }

    private void Update()
    {
        Vector3 newBodyDir = Vector3.RotateTowards(bodyTransform.forward, Vector3.Normalize(agent.steeringTarget - bodyTransform.position), rotationSpeed * Time.deltaTime, 0f);
        newBodyDir.y = bodyTransform.forward.y;
        Debug.DrawRay(bodyTransform.position, newBodyDir, Color.red);
        bodyTransform.rotation = Quaternion.LookRotation(newBodyDir, Vector3.up);

        Vector3 newTurretDir = Vector3.RotateTowards(turretTransform.forward, Vector3.Normalize(agent.pathEndPosition - turretTransform.position), rotationSpeed * Time.deltaTime, 0f);
        newTurretDir.y = turretTransform.forward.y;
        Debug.DrawRay(turretTransform.position, newTurretDir, Color.red);
        turretTransform.rotation = Quaternion.LookRotation(newTurretDir, Vector3.up);
    }
}
