using UnityEngine;
using UnityEngine.AI;

public class ConfigureTurnSpeed : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform torso;

    void Update()
    {
        float value = (1f + Vector3.Dot(torso.forward, Vector3.Normalize(agent.steeringTarget - torso.position))) / 2f;
        agent.speed = 3.5f * curve.Evaluate(value);
    }
}
