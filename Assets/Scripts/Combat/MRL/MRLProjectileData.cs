using UnityEngine;

[CreateAssetMenu(fileName = "MRLProjectileData", menuName = "MechBattle/MRLProjectileData", order = 1)]
public class MRLProjectileData : ProjectileData
{
    public float RotationSpeed;
    public float ExplosionRadius;
    public AnimationCurve TurnControlCurve;
}
