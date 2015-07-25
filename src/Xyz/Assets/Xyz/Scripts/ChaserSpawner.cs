using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Xyz.Scripts;
using Random = UnityEngine.Random;

public class ChaserSpawner : Singleton<ChaserSpawner>
{
    private List<SpawnMarker> _chaserSpawns;
    private GameObject _chaserPrefab;

    public void Start()
    {
        _chaserPrefab = Resources.Load<GameObject>("Prefabs/Chaser");

        _chaserSpawns = 
            FindObjectsOfType<SpawnMarker>()
            .Where(c => c.IsFor(SpawnMarker.Type.Chaser))
            .ToList();
    }

    public void SpawnChasers(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var delay = Random.Range(.5f, 1f);
            StartCoroutine(ExecuteAfterDelay(delay, SpawnChaser));
        }
    }

    private IEnumerator ExecuteAfterDelay(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    private void SpawnChaser()
    {
        var index = Random.Range(0, _chaserSpawns.Count);
        var spawn = _chaserSpawns[index];

        Instantiate(_chaserPrefab, spawn.transform.position, Quaternion.identity);
    }

}
