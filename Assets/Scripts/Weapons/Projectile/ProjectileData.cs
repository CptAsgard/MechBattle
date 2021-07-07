using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "MechBattle/ProjectileData", order = 1)]
public class ProjectileData : ScriptableObject
{
    public float MuzzleVelocity;
    public float DamageOnHit;
}
