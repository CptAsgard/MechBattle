using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private float FireAngleThreshold;

    private void FixedUpdate()
    {
        if (!weapon.Armed)
        {
            return;
        }

        if (Vector3.Angle(transform.forward, weapon.AimDirection) < FireAngleThreshold)
        {
            weapon.Fire();
        }
    }
}
