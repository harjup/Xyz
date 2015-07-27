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
        var mainSessionManager = FindObjectOfType<MainSessionManager>();
        if (mainSessionManager != null)
        {
            mainSessionManager.SetRequiredBeacons(GetRequiredBeacons());
            mainSessionManager.SetChasersPerWave(GetChasersPerWave());
        }
    }

    public int GetRequiredBeacons()
    {
        return 5 + _clearedLevels;
    }

    public int GetChasersPerWave()
    {
        return 2 + _clearedLevels;
    }

    public int GetAudienceMemberCount()
    {
        return 5 + _clearedLevels * 2;
    }

    public void LevelComplete()
    {
        _clearedLevels++;
        Application.LoadLevel(SceneResolver.GetSceneName(SceneResolver.Scene.Result));
    }
}
