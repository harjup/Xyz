using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class BeaconSpawner : Singleton<BeaconSpawner>
{
    private List<SpawnMarker> _beaconSpawns;
    private GameObject _beaconPrefab;

    public void Start()
    {
        _beaconPrefab = Resources.Load<GameObject>("Prefabs/Beacon");

        _beaconSpawns =
            FindObjectsOfType<SpawnMarker>()
            .Where(c => c.IsFor(SpawnMarker.Type.Beacon))
            .ToList();
    }

    public void SpawnBeacons(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var delay = Random.Range(.5f, 1f);
            var spawnRoutine = UsefulCoroutines.ExecuteAfterDelay(delay, SpawnBeaconInAvailableSpot);
            StartCoroutine(spawnRoutine);
        }
    }

    // TODO: Probably not threadsafe
    private void SpawnBeaconInAvailableSpot()
    {
        var availableSpawns = _beaconSpawns.Where(c => c.transform.childCount == 0).ToList();
        var index = Random.Range(0, availableSpawns.Count);
        var spawn = availableSpawns[index];

        var spawnedBeacon = Instantiate(_beaconPrefab, spawn.transform.position, Quaternion.identity) as GameObject;
        spawnedBeacon.transform.parent = spawn.transform;
    }
}
