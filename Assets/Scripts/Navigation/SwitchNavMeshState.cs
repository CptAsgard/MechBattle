using UnityEngine;
using UnityEngine.AI;

public class SwitchNavMeshState : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private NavMeshObstacle obstacle;

    private void Update()
    {
        if (agent.enabled && !agent.hasPath)
        {
            EnableObstacle();
        }
    }

    private void EnableObstacle()
    {
        agent.enabled = false;
        obstacle.enabled = true;
    }
}
