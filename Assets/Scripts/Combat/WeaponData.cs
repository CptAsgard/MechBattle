using UnityEngine;

public enum WeaponType
{
    Projectile,
    Raycast,
    Beam
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "MechBattle/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;
    public float firingSpeed;
    public float range;
    public float damage;
    public float splash;
    public float projectileSpeed;
    public float maxAngleForwardDeviation;
    public int pointsCost;
}
