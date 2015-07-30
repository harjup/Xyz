using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DifficultyManager : Singleton<DifficultyManager>
{
    private int _clearedLevels;
    public int ClearedLevelsInitialValue = 0;

    public void Start()
    {
        if (_clearedLevels == 0)
        {
            _clearedLevels = ClearedLevelsInitialValue;
        }

        DontDestroyOnLoad(gameObject);
        InitializeMainSessionManager();
    }

    public void OnLevelWasLoaded(int level)
    {
        InitializeMainSessionManager();
    }


    private void InitializeMainSessionManager()
    {
        var mainSessionManager = FindObjectOfType<MainSessionManager>();
        if (mainSessionManager != null)
        {
            mainSessionManager.SetRequiredBeacons(GetRequiredBeacons());
            mainSessionManager.SetChasersPerWave(GetChasersPerWave());
            mainSessionManager.SetPushersPerWave(GetPushersPerWave());
        }
    }

    public int GetRequiredBeacons()
    {
        return 5 + (int)_clearedLevels;
    }

    public int GetChasersPerWave()
    {
        return 3 + (int)_clearedLevels / 2;
    }

    public int GetPushersPerWave()
    {
        var value = _clearedLevels;
        if (value > 5)
        {
            value = 5;
        }

        return value;
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
