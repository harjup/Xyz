using UnityEngine;
using System.Collections;

public class MainCanvasManager : Singleton<MainCanvasManager>
{

    private GameObject _mainDisplay;
    private GameObject _winDisplay;
    private GameObject _failureDisplay;

    // Use this for initialization
    void Start()
    {
        _mainDisplay = transform.FindChild("MainDisplay").gameObject;
        _winDisplay = transform.FindChild("WinDisplay").gameObject;
        _failureDisplay = transform.FindChild("FailureDisplay").gameObject;
    }

    public void ShowMainDisplay()
    {
        _mainDisplay.SetActive(true); 
        _failureDisplay.SetActive(false);
        _winDisplay.SetActive(false);
    }

    public void FailFailureDisplay()
    {
        _mainDisplay.SetActive(false);
        _winDisplay.SetActive(false);
        _failureDisplay.SetActive(true);
    }

    public void ShowWinDisplay()
    {
        _winDisplay.SetActive(true);
        _mainDisplay.SetActive(false); 
        _failureDisplay.SetActive(false);
    }

    public void HideAll()
    {
        _winDisplay.SetActive(false);
        _mainDisplay.SetActive(false);
        _failureDisplay.SetActive(false);
    }

}
