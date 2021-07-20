using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WeaponTargetRepository : NetworkBehaviour
{
    public NetworkIdentity OverrideTarget { get; private set; }

    public IEnumerable<NetworkIdentity> Targets => targets;
    public int TargetsCount => targets.Count;
    
    private SyncList<NetworkIdentity> targets = new SyncList<NetworkIdentity>();

    public void SetPriorityTarget(NetworkIdentity target)
    {
        OverrideTarget = target;
        targets.Add(OverrideTarget);
    }

    public void ClearPriorityTarget()
    {
        targets.Remove(OverrideTarget);
        OverrideTarget = null;
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
