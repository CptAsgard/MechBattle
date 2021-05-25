using UnityEngine;

[CreateAssetMenu(fileName = "MechStatsScriptableObject", menuName = "ScriptableObjects/MechStatsScriptableObject", order = 1)]
public class MechStatsScriptableObject : ScriptableObject
{
    public float TorsoHealth;
    public float HeadHealth;
    public float LeftArmHealth;
    public float RightArmHealth;
    public float LeftLegHealth;
    public float RightLegHealth;
}
