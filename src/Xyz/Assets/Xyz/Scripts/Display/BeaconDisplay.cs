using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeaconDisplay : MonoBehaviour
{
    private Text _text;
    private MainSessionManager _sessionManager;

    // Use this for initialization
    void Start()
    {
        _sessionManager = MainSessionManager.Instance;
        _text = GetComponent<Text>();
    }

    public void Update()
    {
        var goal = _sessionManager.BeaconsRequired;
        var current = _sessionManager.BeaconCount;
        _text.text = string.Format("{0:D2}/{1:D2}", current, goal);
    }
}
