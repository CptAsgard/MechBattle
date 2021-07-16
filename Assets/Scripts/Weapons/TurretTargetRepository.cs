using Mirror;

public class TurretTargetRepository : NetworkBehaviour
{
    [field: SyncVar]
    public NetworkIdentity PriorityTarget { get; private set; }

    public void SetPriorityTarget(NetworkIdentity target)
    {
        PriorityTarget = target;
    }
}
