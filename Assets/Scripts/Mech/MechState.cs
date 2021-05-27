using Mirror;

public class MechState : NetworkBehaviour
{
    public NetworkConnection Owner { get; private set; }

    [SyncVar]
    public int PlayerIndex;
    [SyncVar]
    public NetworkIdentity Target;
    [SyncVar]
    public MechPowerState PowerState;

    public void Initialize(NetworkConnection owner, int index)
    {
        Owner = owner;
        PlayerIndex = index;
    }
}
