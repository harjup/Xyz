using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class SpawnMarker : MonoBehaviour {
    public enum Type
    {
        Unknown,
        Player,
        Chaser
    }

    public Type SpawnType;

    public bool IsFor(Type type)
    {
        return SpawnType == type;
    }
}
