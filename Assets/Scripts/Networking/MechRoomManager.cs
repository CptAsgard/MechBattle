using Mirror;
using UnityEngine;

public class MechRoomManager : NetworkRoomManager
{
    [Header("Mech")]
    [SerializeField]
    private GameObject mechPrefab;
    [SerializeField]
    private GameObject projectileWeaponPrefab;
    [SerializeField]
    private MechRepository mechRepository;

    private int index = 0;

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        SpawnPointCollection spawn = gamePlayer.GetComponentInParent<SpawnPointCollection>();

        for (int i = 1; i <= spawn.spawnPoints.Count; i++)
        {
            GameObject mech = Instantiate(mechPrefab, spawn.spawnPoints[i - 1].position, spawn.spawnPoints[i - 1].rotation);
            GameObject weapon = Instantiate(projectileWeaponPrefab);

            NetworkServer.Spawn(mech);
            NetworkServer.Spawn(weapon);

            MechState newMechState = mech.GetComponent<MechState>();
            newMechState.Initialize(conn, index + 1); // NOTE : we start at 1, uninitialized mechs start at 0 and are considered enemies
            mechRepository.Add(newMechState);

            mech.GetComponent<AIPathBlocker>().seekerTag = spawn.spawnPoints.Count * index + i; // TODO : ugly & unreliable
            mech.GetComponent<MechWeaponsController>().Add(weapon.GetComponent<Weapon>());
        }

        gamePlayer.GetComponent<Player>().identity = index + 1;
        index++;

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }
}
