﻿using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class SpawnMarker : MonoBehaviour {
    public enum Type
    {
        Unknown,
        Player,
        Chaser,
        Beacon
    }

    public Type SpawnType;

    public bool IsFor(Type type)
    {
        return SpawnType == type;
    }
}
