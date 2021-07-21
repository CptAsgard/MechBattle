using System.Collections.Generic;
using Mirror;
using Pathfinding;
using UnityEngine;

public class AIPathNetworked : NetworkBehaviour
{
    [SerializeField]
    private Seeker seeker;
    [SerializeField]
    private LineRenderer pathRenderer;

    [SyncVar]
    private List<Vector3> path;

    private void Start()
    {
        pathRenderer.transform.parent = null;
    }

    private void Update()
    {
        if (path == null)
        {
            return;
        }

        pathRenderer.positionCount = path.Count;
        pathRenderer.SetPositions(path.ToArray());
        pathRenderer.SetPosition(0, transform.position);
    }

    public override void OnStartServer()
    {
        seeker.pathCallback += OnPathGenerated;
        enabled = isClient;
    }

    private void OnPathGenerated(Path p)
    {
        path = p.vectorPath;
    }
}
