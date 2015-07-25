using UnityEngine;
using System.Collections;

public class MessageManager : Singleton<MessageManager>
{
    public void ShowMessage(string message)
    {
        Debug.Log(message);
    }

    public void ShowMessage(string message, object[] args)
    {
        ShowMessage(string.Format(message, args));
    }
}
