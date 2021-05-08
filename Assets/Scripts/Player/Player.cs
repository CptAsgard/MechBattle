using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int identity = -1;
}
