using UnityEngine;

public class MRLWeaponView : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
