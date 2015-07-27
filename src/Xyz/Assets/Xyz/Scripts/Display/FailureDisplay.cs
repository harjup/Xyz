using UnityEngine;
using System.Collections;

public class FailureDisplay : MonoBehaviour
{
    public void OnRetryButtonClick()
    {
        Application.LoadLevel(SceneResolver.GetSceneName(SceneResolver.Scene.Main));
    }
}
