using Mirror;

public class Selectable : NetworkBehaviour
{
    [SyncVar]
    public int owner;
}
