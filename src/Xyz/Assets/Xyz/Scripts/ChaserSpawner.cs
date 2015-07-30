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
    private GameObject _pusherPrefab;

    public void Start()
    {
        _chaserPrefab = Resources.Load<GameObject>("Prefabs/Chaser");
        _pusherPrefab = Resources.Load<GameObject>("Prefabs/Chaser-Bumper");

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
            StartCoroutine(ExecuteAfterDelay(delay, SpawnChaser, _chaserPrefab));
        }
    }

    public void SpawnPushers(int amount = 0)
    {
        for (int i = 0; i < amount; i++)
        {
            var delay = Random.Range(.5f, 1f);
            StartCoroutine(ExecuteAfterDelay(delay, SpawnChaser, _pusherPrefab));
        }
    }

    private IEnumerator ExecuteAfterDelay(float time, Action<GameObject> action, GameObject prefab)
    {
        yield return new WaitForSeconds(time);
        action(prefab);
    }

    private void SpawnChaser(GameObject prefab)
    {
        var index = Random.Range(0, _chaserSpawns.Count);
        var spawn = _chaserSpawns[index];

        Instantiate(prefab, spawn.transform.position, Quaternion.identity);
    }
}
