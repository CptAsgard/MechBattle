using Mirror;
using UnityEngine;

public class MechComponentRepository : NetworkBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private MechStatsScriptableObject mechStatsScriptableObject;

    [Header("Bounds")]
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

    [SyncVar]
    private MechComponent torsoComponent;
    [SyncVar]
    private MechComponent headComponent;
    [SyncVar]
    private MechComponent leftArmComponent;
    [SyncVar]
    private MechComponent rightArmComponent;
    [SyncVar]
    private MechComponent leftLegComponent;
    [SyncVar]
    private MechComponent rightLegComponent;

    private void Awake()
    {
        torsoComponent = new MechComponent(MechComponentLocation.Torso, mechStatsScriptableObject.TorsoHealth);
        headComponent = new MechComponent(MechComponentLocation.Head, mechStatsScriptableObject.HeadHealth);
        leftArmComponent = new MechComponent(MechComponentLocation.LeftArm, mechStatsScriptableObject.LeftArmHealth);
        rightArmComponent = new MechComponent(MechComponentLocation.RightArm, mechStatsScriptableObject.RightArmHealth);
        leftLegComponent = new MechComponent(MechComponentLocation.LeftLeg, mechStatsScriptableObject.LeftLegHealth);
        rightLegComponent = new MechComponent(MechComponentLocation.RightLeg, mechStatsScriptableObject.RightLegHealth);
    }

    public MechComponent GetComponent(MechComponentLocation target)
    {
        return target switch
        {
            MechComponentLocation.Torso => torsoComponent,
            MechComponentLocation.Head => headComponent,
            MechComponentLocation.LeftArm => leftArmComponent,
            MechComponentLocation.RightArm => rightArmComponent,
            MechComponentLocation.LeftLeg => leftLegComponent,
            MechComponentLocation.RightLeg => rightLegComponent,
            _ => throw new System.ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public Vector3 GetWorldPosition(MechComponentLocation target)
    {
        return target switch
        {
            MechComponentLocation.Torso => RandomPointInBounds(torsoBounds.bounds),
            MechComponentLocation.Head => RandomPointInBounds(headBounds.bounds),
            MechComponentLocation.LeftArm => RandomPointInBounds(leftArmBounds.bounds),
            MechComponentLocation.RightArm => RandomPointInBounds(rightArmBounds.bounds),
            MechComponentLocation.LeftLeg => RandomPointInBounds(leftLegBounds.bounds),
            MechComponentLocation.RightLeg => RandomPointInBounds(rightLegBounds.bounds),
            _ => throw new System.ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    private static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return bounds.center;
    }
}
