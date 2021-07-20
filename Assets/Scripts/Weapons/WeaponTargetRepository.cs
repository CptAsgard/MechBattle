using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WeaponTargetRepository : NetworkBehaviour
{
    [field: SyncVar]
    public NetworkIdentity PriorityTarget { get; private set; }

    public IEnumerable<NetworkIdentity> Targets => targets;
    public int TargetsCount => targets.Count;
    
    private List<NetworkIdentity> targets = new List<NetworkIdentity>();

    public void SetPriorityTarget(NetworkIdentity target)
    {
        PriorityTarget = target;
        targets.Add(PriorityTarget);
    }

    public void ClearPriorityTarget()
    {
        targets.Remove(PriorityTarget);
        PriorityTarget = null;
    }

    public void Add(NetworkIdentity target)
    {
        targets.Add(target);
        Debug.Log($"new target {target}", target);
    }

    public void Remove(NetworkIdentity target)
    {
        targets.Remove(target);
        Debug.Log($"lost target {target}", target);
    }

    public bool Contains(NetworkIdentity target)
    {
        return targets.Contains(target);
    }
}
