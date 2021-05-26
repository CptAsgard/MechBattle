using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class LineOfSightInterestManagement : InterestManagement
{        
    [Tooltip("Rebuild all every 'rebuildInterval' seconds.")]
    public float rebuildInterval = 1;
    double lastRebuildTime;
    [SerializeField]
    private MechRepository mechRepository;

    public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnection newObserver)
    {
        if (identity.connectionToClient == newObserver)
        {
            return true;
        }

        Player nonRoomPlayer = newObserver.identity.GetComponent<Player>();
        if (nonRoomPlayer == null)
        {
            return false;
        }

        int ownerID = newObserver.identity.GetComponent<Player>().identity;
        var mechs = mechRepository.GetMechsByOwner(ownerID);

        return mechs.Any(mech => mech.GetComponent<MechVisibilityHandler>().CanSee(identity.transform.position));
    }

    public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnection> newObservers, bool initialize)
    {
        Vector3 position = identity.transform.position;

        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn != null && conn.isAuthenticated && conn.identity != null)
            {
                Player nonRoomPlayer = conn.identity.GetComponent<Player>();
                if (nonRoomPlayer == null)
                {
                    continue;
                }

                int ownerID = conn.identity.GetComponent<Player>().identity;
                var mechs = mechRepository.GetMechsByOwner(ownerID);

                if (mechs.Any(mech => mech.GetComponent<MechVisibilityHandler>().CanSee(position)))
                {
                    newObservers.Add(conn);
                }
            }
        }
    }

    void Update()
    {
        // only on server
        if (!NetworkServer.active) return;

        // rebuild all spawned NetworkIdentity's observers every interval
        if (NetworkTime.time >= lastRebuildTime + rebuildInterval)
        {
            RebuildAll();
            lastRebuildTime = NetworkTime.time;
        }
    }
}
