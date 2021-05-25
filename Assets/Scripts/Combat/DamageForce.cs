using UnityEngine;

[CreateAssetMenu("DamageForceScriptableObject", "ScriptableObject/DamageForceScriptableObject", 1)]
public class DamageForceScriptableObject : ScriptableObject, IDamageForce
{
    public float Damage { get; set;  }
}
