using UnityEngine;
using Random = UnityEngine.Random;

public class ComponentsTarget : MonoBehaviour
{
    [SerializeField]
    private BoxCollider torsoBounds;
    [SerializeField]
    private BoxCollider headBounds;
    [SerializeField]
    private BoxCollider leftArmBounds;
    [SerializeField]
    private BoxCollider rightArmBounds;
    [SerializeField]
    private BoxCollider leftLegBounds;
    [SerializeField]
    private BoxCollider rightLegBounds;

    public Vector3 GetWorldPosition(Components target)
    {
        return target switch
        {
            Components.Torso => RandomPointInBounds(torsoBounds.bounds),
            Components.Head => RandomPointInBounds(headBounds.bounds),
            Components.LeftArm => RandomPointInBounds(leftArmBounds.bounds),
            Components.RightArm => RandomPointInBounds(rightArmBounds.bounds),
            Components.LeftLeg => RandomPointInBounds(leftLegBounds.bounds),
            Components.RightLeg => RandomPointInBounds(rightLegBounds.bounds),
            _ => throw new System.ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    private static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return bounds.center;
        //return new Vector3(
        //    Random.Range(bounds.min.x, bounds.max.x),
        //    Random.Range(bounds.min.y, bounds.max.y),
        //    Random.Range(bounds.min.z, bounds.max.z)
        //);
    }
}
