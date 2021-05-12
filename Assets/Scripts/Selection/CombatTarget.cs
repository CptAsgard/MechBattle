using UnityEngine;

public class CombatTarget : MonoBehaviour
{
    [SerializeField]
    private Transform current;

    public Transform Current => current;
}
