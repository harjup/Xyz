using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    private Text _cameraText;
    private Text _guardText;
    private DifficultyManager _difficultyManager;


    public void Start()
    {
        _cameraText = transform.FindChild("CameraText").gameObject.GetComponentSafe<Text>();
        _guardText = transform.FindChild("GuardText").gameObject.GetComponentSafe<Text>();
        _difficultyManager = DifficultyManager.Instance;

        _cameraText.text = string.Format("{0:D2}", _difficultyManager.GetRequiredBeacons());
        _guardText.text = string.Format("{0:D4}", _difficultyManager.GetChasersPerWave() * 40);
    }

    public void OnNextLevelButtonClick()
    {
        Application.LoadLevel(SceneResolver.GetSceneName(SceneResolver.Scene.Main));
    }
}
