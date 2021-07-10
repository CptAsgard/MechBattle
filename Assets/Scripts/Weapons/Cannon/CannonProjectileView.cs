using UnityEngine;

public class CannonProjectileView : MonoBehaviour
{
    [SerializeField]
    private CannonProjectile projectile;
    [SerializeField]
    private TrailRenderer trail;
    
    private void OnDestroy()
    {
        trail.transform.parent = null;
        trail.autodestruct = true;
    }

    private void Awake()
    {
        projectile.OnSpawnEvent += Initialize;
    }

    public void Initialize(Vector3 initialPosition)
    {
        trail.AddPosition(initialPosition);
        projectile.OnStepEvent += OnStepEvent;
    }

    private void OnStepEvent(Vector3 currentPosition)
    {
        trail.emitting = true;
    }
}
