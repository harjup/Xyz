using UnityEngine;
using System.Collections;

public class MainTitleMenu : MonoBehaviour
{
    private GameObject _title;
    private GameObject _credit;
    private GameObject _control;

    // Use this for initialization
    void Start()
    {
        // Options.CensoredMode = true;

        var censoredMode = Options.CensoredMode;

        _title = censoredMode 
            ? transform.FindChild("Title-Censored").gameObject 
            : transform.FindChild("Title").gameObject;
        _credit = transform.FindChild("Credit").gameObject;
        _control = transform.FindChild("Control").gameObject;

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
    public void OnControlsButtonClick()
    {
        ShowControl();
    }

    public void OnBackButtonClick()
    {
        ShowTitle();
    }

    private void ShowTitle()
    {
        _title.SetActive(true);
        _credit.SetActive(false);
        _control.SetActive(false);
    }

    private void ShowCredits()
    {
        _title.SetActive(false);
        _credit.SetActive(true);
        _control.SetActive(false);
    }

    private void ShowControl()
    {
        _control.SetActive(true);
        _title.SetActive(false);
        _credit.SetActive(false);
    }
}
