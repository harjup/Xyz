using UnityEngine;
using System.Collections;

public class MainCanvasManager : Singleton<MainCanvasManager>
{

    private GameObject _mainDisplay;
    private GameObject _failureDisplay;

    // Use this for initialization
    void Start()
    {
        _mainDisplay = transform.FindChild("MainDisplay").gameObject;
        _failureDisplay = transform.FindChild("FailureDisplay").gameObject;
    }

    public void ShowMainDisplay()
    {
        _mainDisplay.SetActive(true); 
        _failureDisplay.SetActive(false);
    }

    public void FailFailureDisplay()
    {
        _mainDisplay.SetActive(false);
        _failureDisplay.SetActive(true);
    }

}
