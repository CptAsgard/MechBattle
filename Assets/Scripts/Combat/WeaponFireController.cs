using UnityEngine;

public class WeaponFireController : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;

    public float Cooldown { get; private set; }

    public bool ReadyToFire => Cooldown <= 0f;

    private void Start()
    {
        Cooldown = 0f;
    }

    private void Update()
    {
        if (Cooldown > 0f)
        {
            Cooldown -= Time.deltaTime;
        }
    }

    public void ResetCooldown()
    {
        Cooldown = weapon.WeaponData.reloadDelay;
    }
}
