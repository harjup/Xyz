using UnityEngine;
using System.Collections;

public class MessageManager : Singleton<MessageManager>
{
    private MessageDisplay _messageDisplay;

    public void Start()
    {
        _messageDisplay = FindObjectOfType<MessageDisplay>();
    }

    public void ShowMessage(string message)
    {
        _messageDisplay.SetText(message);
        _messageDisplay.TweenMessageOnAndOffScreen();
    }

    public void ShowMessage(string message, object[] args)
    {
        ShowMessage(string.Format(message, args));
    }
}
