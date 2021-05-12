using UnityEngine;

public enum WeaponType
{
    Projectile,
    Raycast,
    Beam
}

public abstract class WeaponData : ScriptableObject
{
    public abstract WeaponType weaponType { get; }
    public int pointsCost;
    public float maxAngleDeviation;
    public float reloadDelay;
}
