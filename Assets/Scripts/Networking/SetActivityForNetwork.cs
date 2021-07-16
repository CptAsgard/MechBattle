using System.Collections.Generic;
using Mirror;
using UnityEngine;

// TODO replace this hack with a preprocessing step with interfaces to delete/disable unnecessary components
public class SetActivityForNetwork : NetworkBehaviour
{
    [SerializeField]
    private bool destroyInstead = false;
    [SerializeField]
    private List<MonoBehaviour> clientOnly;
    [SerializeField]
    private List<MonoBehaviour> serverOnly;
    [SerializeField]
    private List<MonoBehaviour> authorityOnly;

    private void Start()
    {
        foreach (MonoBehaviour behaviour in clientOnly)
        {
            if (destroyInstead && !isClient) Destroy(behaviour); else behaviour.enabled = isClient;
        }

        foreach (MonoBehaviour behaviour in serverOnly)
        {
            if (destroyInstead && !isServer) Destroy(behaviour); else behaviour.enabled = isServer;
        }

        foreach (MonoBehaviour behaviour in authorityOnly)
        {
            if (destroyInstead && !hasAuthority) Destroy(behaviour); else behaviour.enabled = hasAuthority;
        }
    }
}
