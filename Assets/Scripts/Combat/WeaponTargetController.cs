using Mirror;
using UnityEngine;

public class WeaponTargetController : MonoBehaviour
{
    [SerializeField]
    private WeaponTargetRepository repository;

    public NetworkIdentity GetPriorityTarget()
    {
        return repository.PriorityTarget;
    }
}
