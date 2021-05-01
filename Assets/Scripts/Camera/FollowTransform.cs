using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}
