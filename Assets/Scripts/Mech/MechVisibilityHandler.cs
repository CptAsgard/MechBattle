using UnityEngine;

public class MechVisibilityHandler : MonoBehaviour
{
    [SerializeField]
    private Transform sensorsTransform;
    [SerializeField]
    private LayerMask blockingMask;

    public bool CanSee(Vector3 point)
    {
        Vector3 center = point + Vector3.up;

        return Test(sensorsTransform.position, center + Vector3.left) ||
                Test(sensorsTransform.position, center + Vector3.right) ||
                Test(sensorsTransform.position, center + Vector3.up) ||
                Test(sensorsTransform.position, center + Vector3.down);
    }

    private bool Test(Vector3 a, Vector3 b)
    {
        return !Physics.Linecast(a, b, blockingMask);
    }
}
