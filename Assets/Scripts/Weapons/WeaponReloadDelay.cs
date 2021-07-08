using UnityEngine;

public class WeaponReloadDelay : MonoBehaviour
{
    [SerializeField]
    private WeaponController weaponController;

    public bool ReadyToFire => timer <= 0f;
    public float TimeLeft => timer;

    private float timer;

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void ResetCooldown()
    {
        timer = weaponController.WeaponData.ReloadDelay;
    }
}
