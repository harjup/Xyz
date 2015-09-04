using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public void CensorChoiceClick(bool choice)
    {
        Options.CensoredMode = choice;

        Application.LoadLevel("Title");
    }
}
