using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DifficultyManager : Singleton<DifficultyManager>
{
    private int _clearedLevels;

    public void Start()
    {
        _clearedLevels = 0;
        DontDestroyOnLoad(gameObject);
    }

    public void OnLevelWasLoaded(int level)
    {
        var mainSessionManager = MainSessionManager.Instance;
        if (mainSessionManager != null)
        {
            mainSessionManager.SetDifficulty(_clearedLevels);
        }

    }

    public void LevelComplete()
    {
        _clearedLevels++;
        Application.LoadLevel("Sandbox");
    }
}
