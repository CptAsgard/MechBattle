using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;
    [SerializeField]
    private TrailRenderer trail;
    
    private void OnDestroy()
    {
        trail.transform.parent = null;
        trail.autodestruct = true;
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
