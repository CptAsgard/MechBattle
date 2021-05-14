using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;

    void Update()
    {
        transform.position = followTarget.position;
    }
}
