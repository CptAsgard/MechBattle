using System.Collections.Generic;
using System.Linq;
using Mirror;

public class TargetsRepository : SingletonComponent<TargetsRepository>
{
    private List<NetworkIdentity> targets = new List<NetworkIdentity>();

    public IEnumerable<NetworkIdentity> Targets => targets.Where(target => target != null);

    public void Add<T>(T target) where T : NetworkBehaviour, ITarget
    {
        targets.Add(target.netIdentity);
    }

    public void Remove<T>(T target) where T : NetworkBehaviour, ITarget
    {
        targets.Remove(target.netIdentity);
    }

    public void Remove(NetworkIdentity target)
    {
        targets.Remove(target);
    }

    public void ClearTargets()
    {
        targets.Clear();
    }
}

public interface ITarget
{ }
