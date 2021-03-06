using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class LineOfSightInterestManagement : InterestManagement
{        
    [SerializeField, Tooltip("Rebuild all every 'rebuildInterval' seconds.")]
    private float rebuildInterval = 1;

    private double lastRebuildTime;

    public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnection newObserver)
    {
        if (identity.connectionToClient == newObserver || newObserver.clientOwnedObjects.Contains(identity))
        {
            return true;
        }
        
        var mechs = MechRepository.Instance.GetByOwner(newObserver);
        return mechs.Any(mech => mech.GetComponent<MechVisibilityHandler>().CanSee(identity));
    }

    public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnection> newObservers, bool initialize)
    {
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (identity.connectionToClient == conn || conn.clientOwnedObjects.Contains(identity) || LineOfSightIgnoredRepository.Instance != null && LineOfSightIgnoredRepository.Instance.Contains(identity))
            {
                newObservers.Add(conn);
                continue;
            }

            if (conn.isAuthenticated && conn.identity != null)
            {
                var mechs = MechRepository.Instance.GetByOwner(conn);
                if (mechs.Any(mech => mech.GetComponent<MechVisibilityHandler>().CanSee(identity)))
                {
                    newObservers.Add(conn);
                }
            }
        }
    }

    void Update()
    {
        if (!NetworkServer.active)
        {
            return;
        }

        if (NetworkTime.time >= lastRebuildTime + rebuildInterval)
        {
            RebuildAll();
            lastRebuildTime = NetworkTime.time;
        }
    }
}
