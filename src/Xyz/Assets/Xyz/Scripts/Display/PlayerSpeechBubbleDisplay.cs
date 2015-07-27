using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpeechBubbleDisplay : Singleton<PlayerSpeechBubbleDisplay>
{
    private GameObject _speechBubble;
    private Text _speechBubbleText;
    private GameObject _waggleGraphic;

    // Use this for initialization
    void Start()
    {
        _speechBubble = transform.FindChild("SpeechBubble").gameObject;

        _speechBubbleText = _speechBubble
            .transform
            .FindChild("SpeechBubbleText")
            .gameObject
            .GetComponentSafe<Text>();

        _waggleGraphic = transform.FindChild("Waggle").gameObject;

        _speechBubble.SetActive(false);
        _waggleGraphic.SetActive(false);
    }

    private IEnumerator _displayTextRoutine;
    public void DisplayText(string text)
    {
        if (_displayTextRoutine != null)
        {
            StopCoroutine(_displayTextRoutine);
        }

        _displayTextRoutine = DisplayTextRoutine(text); 
        StartCoroutine(_displayTextRoutine);
    }

    public void DisplayWaggleGraphic()
    {
        _waggleGraphic.SetActive(true);
    }

    public void HideWaggleGraphic()
    {
        _waggleGraphic.SetActive(false);
    }


    private IEnumerator DisplayTextRoutine(string text)
    {
        _speechBubbleText.text = text;
        _speechBubble.SetActive(true);
        yield return new WaitForSeconds(2f);
        _speechBubble.SetActive(false);
    }
}
