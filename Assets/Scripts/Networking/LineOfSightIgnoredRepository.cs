using System.Collections.Generic;
using Mirror;

public class LineOfSightIgnoredRepository : SingletonComponent<LineOfSightIgnoredRepository>
{
    private readonly List<NetworkIdentity> ignoredIdentities = new List<NetworkIdentity>();

    public void Add(NetworkIdentity identity) => ignoredIdentities.Add(identity);
    public void Remove(NetworkIdentity identity) => ignoredIdentities.Remove(identity);
    public bool Contains(NetworkIdentity identity) => ignoredIdentities.Contains(identity);
}
