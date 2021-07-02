using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MRLWeapon : Weapon
{
    [SerializeField]
    private MRLProjectile clientProjectile;
    [SerializeField]
    private MRLProjectile serverProjectile;
    [SerializeField]
    private MRLWeaponData weaponData;
    [SerializeField]
    private WeaponReloadDelay reloadDelay;
    [SerializeField]
    private int maxRockets;
    [SerializeField]
    private List<Transform> launchTubeEnds;

    public override WeaponData WeaponData => weaponData;
    public override bool Armed => reloadDelay.ReadyToFire;
    public override bool AutoAim => false;

    private int rocketsRemaining;
    private float launchSpacingDelay;

    private void Start()
    {
        rocketsRemaining = maxRockets;
    }

    private void FixedUpdate()
    {
        if (!isServer)
        {
            enabled = false;
            return;
        }

        if (!Owner || !Armed || TargetRepository.PriorityTarget == null)
        {
            return;
        }

        if (launchSpacingDelay > 0f)
        {
            launchSpacingDelay -= Time.fixedDeltaTime;
            return;
        }

        Fire();
    }

    [Server]
    public override void Fire()
    {
        Transform target = TargetRepository.PriorityTarget.GetComponent<MechComponentRepository>()
            .GetTransform(MechComponentLocation.Torso);

        Transform launchTubeEnd = launchTubeEnds[rocketsRemaining - 1];

        SpawnRocket(serverProjectile.gameObject, launchTubeEnd.position, launchTubeEnd.up, target);
        RpcSpawnRocket(rocketsRemaining - 1, TargetRepository.PriorityTarget, MechComponentLocation.Torso);

        rocketsRemaining--;
        if (rocketsRemaining <= 0)
        {
            reloadDelay.ResetCooldown();
            rocketsRemaining = maxRockets;
        }
        else
        {
            launchSpacingDelay = weaponData.LaunchSpacingDelay;
        }
    }

    [ClientRpc]
    private void RpcSpawnRocket(int index, NetworkIdentity enemy, MechComponentLocation location)
    {
        Transform origin = launchTubeEnds[index];
        Transform target = enemy.GetComponent<MechComponentRepository>().GetTransform(location);
        SpawnRocket(clientProjectile.gameObject, origin.position, origin.up, target);
    }

    private static void SpawnRocket(GameObject prefab, Vector3 position, Vector3 forward, Transform target)
    {
        GameObject pr = Instantiate(prefab);
        pr.GetComponent<MRLProjectile>().Initialize(position, forward, target);
    }
}
