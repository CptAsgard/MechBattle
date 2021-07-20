using Mirror;

public class MechState : NetworkBehaviour, ITarget
{
    public NetworkConnection Owner { get; private set; }

    [SyncVar]
    public int PlayerIndex;
    [SyncVar]
    public MechPowerState PowerState;

    public override void OnStartServer()
    {
        if (isServer)
        {
            TargetsRepository.Instance.Add(this);
        }
    }
    
    private void OnDestroy()
    {
        if (isServer)
        {
            TargetsRepository.Instance.Remove(this);
        }
    }

    public void Initialize(NetworkConnection owner, int index)
    {
        Owner = owner;
        PlayerIndex = index;
    }
}
