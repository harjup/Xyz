using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainSessionManager : Singleton<MainSessionManager>
{
    public void Start()
    {
        // Move player to random spawn point
        var player = FindObjectOfType<Player>();

        var spawn = 
            FindObjectsOfType<SpawnMarker>()
            .Where(m => m.IsFor(SpawnMarker.Type.Player))
            .ToList()
            .AsRandom()
            .First();

        player.ResetAt(spawn.transform.position);
    }
}
