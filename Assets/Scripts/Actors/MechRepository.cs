using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// TODO : great now we need to reset this list when we reset the game state :(
public class MechRepository : MonoBehaviour
{
    private List<MechState> mechs = new List<MechState>();

    public IEnumerable<MechState> GetMechsByOwner(NetworkConnection owner)
    {
        return mechs.Where(mech => mech.Owner == owner);
    }

    public void Add(MechState mech)
    {
        mechs.Add(mech);
    }
}
