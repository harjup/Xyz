using UnityEngine;
using System.Collections;

public class MainTitleMenu : MonoBehaviour
{
    private GameObject _title;
    private GameObject _credit;

    // Use this for initialization
    void Start()
    {
        _title = transform.FindChild("Title").gameObject;
        _credit = transform.FindChild("Credit").gameObject;

        ShowTitle();
    }

    public void OnStartButtonClick()
    {
        Application.LoadLevel(SceneResolver.GetSceneName(SceneResolver.Scene.Intro));
    }

    public void OnCreditsButtonClick()
    {
        ShowCredits();
    }

    public void OnBackButtonClick()
    {
        ShowTitle();
    }

    private void ShowTitle()
    {
        _title.SetActive(true);
        _credit.SetActive(false);
    }

    private void ShowCredits()
    {
        _title.SetActive(false);
        _credit.SetActive(true);
    }
}
