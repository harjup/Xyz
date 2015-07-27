using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextCrawlDisplay : Singleton<TextCrawlDisplay>
{
    private const int MaxPageLength = 200;
    private string _fullDisplayText;
    private string _currentDisplayText;
    private int _displayIndex = 1;
    private bool _inProcess;

    private Text _text;

    private IEnumerator _textDisplayRoutine = null;


    public void Start()
    {
        _text = GetComponentInChildren<Text>();
    }

    public IEnumerator TextCrawl(string text)
    {
        InitText(text);

        _textDisplayRoutine = CrawlText();
        yield return StartCoroutine(CrawlText());
        _textDisplayRoutine = null;
    }

    private void InitText(string text)
    {
        _fullDisplayText = text;
        _currentDisplayText = "";
        _displayIndex = 0;

        if (_textDisplayRoutine != null)
        {
            StopCoroutine(_currentDisplayText);
        }
    }


    IEnumerator CrawlText()
    {
        _inProcess = true;

        while (_displayIndex <= _fullDisplayText.Length)
        {
            _currentDisplayText = _fullDisplayText.Substring(0, _displayIndex);
            _displayIndex += 1;

            _text.text = _currentDisplayText;

            yield return new WaitForSeconds(.025f);
        }

        _inProcess = false;
    }
}
