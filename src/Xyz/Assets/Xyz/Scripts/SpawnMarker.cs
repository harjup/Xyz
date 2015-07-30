using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class SpawnMarker : MonoBehaviour {
    public enum Type
    {
        Unknown,
        Player,
        Chaser,
        Beacon,
        Pusher
    }

    public Type SpawnType;

    public bool IsFor(Type type)
    {
        return SpawnType == type;
    }

    public void Start()
    {
        gameObject.GetComponentAndExecuteIfExists<Renderer>((r) => r.enabled = false);
    }
}
