using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    private Text _messageText;
    private Sequence _currentSequence;


    void Start()
    {
        _messageText = GetComponentInChildren<Text>();
        _messageText.text = "";
    }

    public void SetText(string text)
    {
        _messageText.text = text;
    }

    public void TweenMessageOnAndOffScreen()
    {
        _currentSequence = DOTween
            .Sequence()
            .Append(transform.DOLocalMoveY(-120f, .5f))
            .AppendInterval(2.5f)
            .Append(transform.DOLocalMoveY(-380f, .5f))
            .Play();
    }


}
