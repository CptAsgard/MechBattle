using UnityEngine;

public class MechVisibilityHandler : MonoBehaviour
{
    public bool CanSee(Vector3 point)
    {
        return Vector3.Distance(point, transform.position) < 10f;
    }
}
