using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PushCoordinator : Singleton<PushCoordinator>
{
    public int MaxPusherCount { get; private set; }
    
    private List<SpawnMarker> _pushingSpots;

    void Start()
    {
        _pushingSpots = 
            FindObjectsOfType<SpawnMarker>()
            .Where(s => s.IsFor(SpawnMarker.Type.Pusher))
            .ToList();

        MaxPusherCount = _pushingSpots.Count;
    }

    public Vector3 GetPushingSpot()
    {
        if (_pushingSpots.Count == 0)
        {
            return Vector3.zero;
        }

        lock (_pushingSpots)
        {
            var value = _pushingSpots.GetRandom();
            //_pushingSpots.Remove(value);
            return value.transform.position;
        }
    }

}
