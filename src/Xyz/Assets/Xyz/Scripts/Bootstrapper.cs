using UnityEngine;
using System.Collections;

public class Bootstrapper : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        var difficultyManager = FindObjectOfType<DifficultyManager>();
        if (difficultyManager == null)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/DifficultyManager");
            var result = Instantiate(prefab);
            result.name = "DifficultyManager";
        }
    }

}
