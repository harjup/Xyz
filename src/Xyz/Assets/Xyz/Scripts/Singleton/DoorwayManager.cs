using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DoorwayManager : Singleton<DoorwayManager>
{
    private List<Doorway> _doorways;

    void Start()
    {
        _doorways = FindObjectsOfType<Doorway>().ToList();
    }

    public void EnableAllExits()
    {
        _doorways.ForEach(d => d.EnableExit());
    }
}
