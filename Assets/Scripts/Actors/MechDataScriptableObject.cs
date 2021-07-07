using UnityEngine;

[CreateAssetMenu(fileName = "MechDataScriptableObject", menuName = "ScriptableObjects/MechDataScriptableObject", order = 1)]
public class MechDataScriptableObject : ScriptableObject
{
    [Header("Components")]
    public float TorsoHealth;
    public float HeadHealth;
    public float LeftArmHealth;
    public float RightArmHealth;
    public float LeftLegHealth;
    public float RightLegHealth;
    
    [Header("Stats")]
    public float RotationSpeed;
}
