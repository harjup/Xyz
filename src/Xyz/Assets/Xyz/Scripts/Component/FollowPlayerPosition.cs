using UnityEngine;
using System.Collections;

public class FollowPlayerPosition : MonoBehaviour
{
    private GameObject _playerSpeedBubble;

    public void Start()
    {
        _playerSpeedBubble = GameObject.Find("SpeechBubbleCenter").gameObject;
    }

    public void Update()
    {
        transform.position = _playerSpeedBubble.transform.position;
    }
}
