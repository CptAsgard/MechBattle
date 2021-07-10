using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public System.Action<Vector3> OnSpawnEvent;
    public System.Action<Vector3> OnStepEvent;
    public System.Action<IDamageable, Vector3> OnHitEvent;

    [SerializeField]
    protected LayerMask layerMask;
    
    protected Vector3 currentPosition;
    protected TimeSince sinceSpawn;
    protected bool hasAuthority;

    public abstract ProjectileData ProjectileData { get; }

    protected void Awake()
    {
        sinceSpawn = 0f;
    }

    public virtual void Initialize(Vector3 initialPosition, bool withAuthority)
    {
        currentPosition = initialPosition;
        hasAuthority = withAuthority;

        OnSpawnEvent?.Invoke(initialPosition);
    }
}
