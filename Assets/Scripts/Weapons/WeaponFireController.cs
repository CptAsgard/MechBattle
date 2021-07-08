using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    [SerializeField]
    private WeaponController weaponController;
    [SerializeField]
    private float FireAngleThreshold;

    private void FixedUpdate()
    {
        if (!weaponController.Armed)
        {
            return;
        }

        if (Vector3.Angle(transform.forward, weaponController.AimDirection) < FireAngleThreshold)
        {
            weaponController.Fire();
        }
    }
}
