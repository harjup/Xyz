using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpeechBubbleDisplay : Singleton<PlayerSpeechBubbleDisplay>
{
    private GameObject _speechBubble;
    private Text _speechBubbleText;


    // Use this for initialization
    void Start()
    {
        _speechBubble = transform.FindChild("SpeechBubble").gameObject;

        _speechBubbleText = _speechBubble
            .transform
            .FindChild("SpeechBubbleText")
            .gameObject
            .GetComponentSafe<Text>();

        
        _speechBubble.SetActive(false);
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


    private IEnumerator DisplayTextRoutine(string text)
    {
        _speechBubbleText.text = text;
        _speechBubble.SetActive(true);
        yield return new WaitForSeconds(2f);
        _speechBubble.SetActive(false);
    }
}
