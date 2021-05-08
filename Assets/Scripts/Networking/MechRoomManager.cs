using Mirror;
using UnityEngine;

public class MechRoomManager : NetworkRoomManager
{
    [Header("Mech")]
    [SerializeField]
    private GameObject mechPrefab;

    private int index = 0;

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        SpawnPointCollection spawn = gamePlayer.GetComponentInParent<SpawnPointCollection>();

        for (int i = 1; i <= spawn.spawnPoints.Count; i++)
        {
            GameObject mech = Instantiate(mechPrefab, spawn.spawnPoints[i - 1].position, spawn.spawnPoints[i - 1].rotation);
            mech.GetComponentInChildren<UpdateGridSeekerBlock>().seekerTag = spawn.spawnPoints.Count * index + i;
            mech.GetComponent<Selectable>().owner = index;

            NetworkServer.Spawn(mech);
        }

        gamePlayer.GetComponent<Player>().identity = index;
        index++;

        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }
}
