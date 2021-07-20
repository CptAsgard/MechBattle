using System.Collections.Generic;
using System.Linq;
using Mirror;

public class MechRepository : SingletonComponent<MechRepository>
{
    private List<MechState> mechs = new List<MechState>();

    public IEnumerable<MechState> Mechs => mechs;

    public IEnumerable<MechState> GetFriendly(int playerIndex)
    {
        return mechs.Where(mech => mech.PlayerIndex == playerIndex);
    }

    public IEnumerable<MechState> GetEnemy(int playerIndex)
    {
        return mechs.Where(mech => mech.PlayerIndex != playerIndex);
    }

    public IEnumerable<MechState> GetByOwner(NetworkConnection owner)
    {
        return mechs.Where(mech => mech.Owner == owner);
    }

    public void Add(MechState mech)
    {
        mechs.Add(mech);
    }
}
