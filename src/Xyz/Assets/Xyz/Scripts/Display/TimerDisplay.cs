using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    private Text _text;
    private GameTimer _gameTimer;

    // Use this for initialization
    void Start()
    {
        _gameTimer = GameTimer.Instance;
        _text = GetComponent<Text>();
    }

    public void Update()
    {
        var seconds = _gameTimer.GetSeconds();
        var timeSpan = TimeSpan.FromSeconds(seconds);
        _text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
