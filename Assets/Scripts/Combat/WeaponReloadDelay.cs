using UnityEngine;

public class WeaponReloadDelay : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;

    public bool ReadyToFire => timer <= 0f;

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
        timer = weapon.WeaponData.ReloadDelay;
    }
}
