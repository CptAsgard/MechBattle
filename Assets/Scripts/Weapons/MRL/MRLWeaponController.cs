using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MRLWeaponController : WeaponController
{
    [SerializeField]
    private MRLProjectile clientProjectile;
    [SerializeField]
    private MRLProjectile serverProjectile;
    [SerializeField]
    private MRLWeaponData weaponData;
    [SerializeField]
    private int maxRockets;
    [SerializeField]
    private List<Transform> launchTubeEnds;
    
    public override WeaponData WeaponData => weaponData;
    public override bool Armed => reloadTime > weaponData.ReloadDelay;
    public override bool AutoAim => false;

    private int rocketsRemaining;
    private float launchSpacingDelay;

    [Server]
    private void Start()
    {
        rocketsRemaining = maxRockets;
    }
    
    [Server]
    private void FixedUpdate()
    {
        if (!Owner || targetRepository.PriorityTarget == null)
        {
            return;
        }
        
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

        if (weaponData.ReloadDelay - reloadTime <= 1f && !LineOfSightIgnoredRepository.Instance.Contains(netIdentity))
        {
            LineOfSightIgnoredRepository.Instance.Add(netIdentity);
        }

        if (!Armed)
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
        Transform target = targetRepository.PriorityTarget.GetComponent<MechComponentRepository>()
            .GetTransform(MechComponentLocation.Torso);

        Transform launchTubeEnd = launchTubeEnds[rocketsRemaining - 1];

        SpawnRocket(serverProjectile.gameObject, launchTubeEnd.position, launchTubeEnd.up, target, true);
        RpcSpawnRocket(rocketsRemaining - 1, targetRepository.PriorityTarget, MechComponentLocation.Torso);

        rocketsRemaining--;
        if (rocketsRemaining <= 0)
        {
            reloadTime = 0f;
            rocketsRemaining = maxRockets;
            LineOfSightIgnoredRepository.Instance.Remove(netIdentity);
        }
        else
        {
            launchSpacingDelay = weaponData.LaunchSpacingDelay;
        }
    }

    [ClientRpc]
    private void RpcSpawnRocket(int index, NetworkIdentity enemy, MechComponentLocation location)
    {
        Transform launchTubeEnd = launchTubeEnds[index];
        Transform target = enemy.GetComponent<MechComponentRepository>().GetTransform(location);
        SpawnRocket(clientProjectile.gameObject, launchTubeEnd.position, launchTubeEnd.up, target, false);
    }

    private static void SpawnRocket(GameObject prefab, Vector3 position, Vector3 forward, Transform target, bool withAuthority)
    {
        GameObject pr = Instantiate(prefab);
        pr.GetComponent<MRLProjectile>().Initialize(position, forward, target, withAuthority);
    }
}
