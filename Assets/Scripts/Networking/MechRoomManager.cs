using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MechRoomManager : NetworkRoomManager
{
    [Header("Mech")]
    [SerializeField]
    private GameObject mechPrefab;
    [SerializeField]
    private GameObject projectileWeaponPrefab;

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
            
            mech.GetComponent<AIPathBlocker>().seekerTag = spawn.spawnPoints.Count * index + i; // TODO : ugly & unreliable
            mech.GetComponent<MechState>().Initialize(index + 1); // NOTE : default placed mechs will be enemies
            mech.GetComponent<MechWeaponsController>().Add(weapon.GetComponent<Weapon>());
        }

        gamePlayer.GetComponent<Player>().identity = index + 1;
        index++;

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }
}
