using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class IntroCutsceneCanvas : MonoBehaviour
{

    private TextCrawlDisplay _textCrawlDisplay;

    // Use this for initialization
    void Start()
    {
        // Might as well text crawl
        // Show line
        // Wait 5 seconds
        // show next line

        _textCrawlDisplay = TextCrawlDisplay.Instance;
        StartCoroutine(ShowIntro());
    }


    private IEnumerator ShowIntro()
    {
        List<string> dialogs = new List<string>
        {
            "In a recent news story, it was reported that demand for security positions have been dwindling.",
            "Eric Eiselhauser has just finished listening this report in the nude, a Sunday tradition.",
            "I can't believe this! Less security!",
            "What if some weirdo showed up at the sports game! They could do whatever they pleased!",
            "...",
            "I know!",
            "I'll protest by running around the field!",
            "If enough people can see a normal guy running in view on all the cameras in the field, they'll understand the current security risks!",
            "They can hire more staff on before some weirdo tries something!",
            "Now let's get dressed and get going.",
            "Whoops, almost forgot my tie. I'd look silly, without it!",
        };

        // Sometimes there will be image swaps
        // At the end a level gets loaded
        // Clicking on text box also advances

        foreach (var text in dialogs)
        {
            yield return StartCoroutine(_textCrawlDisplay.TextCrawl(text));
            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(3f);

        LoadMainScene();
    }

    public void SkipCutscene()
    {
        LoadMainScene();
    }

    public void LoadMainScene()
    {
        Application.LoadLevel(SceneResolver.GetSceneName(SceneResolver.Scene.Main));
    }

}
