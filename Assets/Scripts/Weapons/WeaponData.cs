using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "MechBattle/WeaponData (Base)", order = 1)]
public class WeaponData : ScriptableObject
{
    public int PointsCost;
    public float ReloadDelay;
}
