using System;
using Mirror;
using UnityEngine;

public class MechComponentRepository : NetworkBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private MechDataScriptableObject mechDataScriptableObject;

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

    private Array enumValues;

    private void Awake()
    {
        torsoComponent = new MechComponent(MechComponentLocation.Torso, mechDataScriptableObject.TorsoHealth);
        headComponent = new MechComponent(MechComponentLocation.Head, mechDataScriptableObject.HeadHealth);
        leftArmComponent = new MechComponent(MechComponentLocation.LeftArm, mechDataScriptableObject.LeftArmHealth);
        rightArmComponent = new MechComponent(MechComponentLocation.RightArm, mechDataScriptableObject.RightArmHealth);
        leftLegComponent = new MechComponent(MechComponentLocation.LeftLeg, mechDataScriptableObject.LeftLegHealth);
        rightLegComponent = new MechComponent(MechComponentLocation.RightLeg, mechDataScriptableObject.RightLegHealth);

        enumValues = Enum.GetValues(typeof(MechComponentLocation));
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

    public Bounds GetBounds(MechComponentLocation target)
    {
        return target switch
        {
            MechComponentLocation.Torso => torsoBounds.bounds,
            MechComponentLocation.Head => headBounds.bounds,
            MechComponentLocation.LeftArm => leftArmBounds.bounds,
            MechComponentLocation.RightArm => rightArmBounds.bounds,
            MechComponentLocation.LeftLeg => leftLegBounds.bounds,
            MechComponentLocation.RightLeg => rightLegBounds.bounds,
            _ => throw new System.ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public Vector3 GetWorldPosition(MechComponentLocation target)
    {
        return GetBounds(target).center;
    }

    public MechComponentLocation GetNearestComponent(Vector3 position)
    {
        float nearestDistance = float.MaxValue;
        MechComponentLocation nearestLocation = MechComponentLocation.Torso;
        foreach (MechComponentLocation location in enumValues)
        {
            float distance = DistanceToPoint(GetBounds(location), position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestLocation = location;
            }
        }
        return nearestLocation;
    }

    private static float DistanceToPoint(Bounds bounds, Vector3 position)
    {
        Vector3 posA = bounds.ClosestPoint(position);
        return (position - posA).sqrMagnitude;
    }
}
