using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class IntroCutsceneCanvas : MonoBehaviour
{

    private TextCrawlDisplay _textCrawlDisplay;

    private class CutsceneDialog
    {
        public CutsceneDialog(string text, Sprite sprite = null)
        {
            Text = text;
            Sprite = sprite;
        }

        public string Text { get; private set; }
        public Sprite Sprite { get; private set; }
    }

    private List<CutsceneDialog> dialogs;
    private Image _image;

    // Use this for initialization
    void Start()
    {
        _image = transform.FindChild("Image").GetComponent<Image>();

        var frameOne = Resources.Load<Sprite>("Cards/Placeholder-01");
        var frameTwo = Resources.Load<Sprite>("Cards/Placeholder-02");
        var frameThree = Resources.Load<Sprite>("Cards/Placeholder-03");



        dialogs = new List<CutsceneDialog>
        {
            new CutsceneDialog(
                "In a recent news story, it was reported that demand for security positions have been dwindling.", 
                frameOne),            

            new CutsceneDialog(
                "Eric Eiselhauser has just finished listening this report in the nude, a Sunday tradition.",
                frameTwo
                ),
            new CutsceneDialog("I can't believe this! Less security!"),
            new CutsceneDialog("What if some weirdo showed up at the sports game! They could do whatever they pleased!"),
            new CutsceneDialog("..."),

            new CutsceneDialog("I know!", frameThree),

            new CutsceneDialog("I'll protest by running around the field!"),
            new CutsceneDialog("If enough people can see a normal guy running in view on all the cameras in the field, they'll understand the current security risks!"),
            new CutsceneDialog("They can hire more staff on before some weirdo tries something!"),
            new CutsceneDialog("Now let's get dressed and get going."),
            new CutsceneDialog("Whoops, almost forgot my tie. I'd look silly, without it!")
        };

        // Might as well text crawl
        // Show line
        // Wait 5 seconds
        // show next line

        _textCrawlDisplay = TextCrawlDisplay.Instance;
        StartCoroutine(ShowIntro());
    }


    private IEnumerator ShowIntro()
    {
        // Sometimes there will be image swaps
        // At the end a level gets loaded
        // Clicking on text box also advances

        foreach (var dialog in dialogs)
        {
            if (dialog.Sprite != null)
            {
                _image.sprite = dialog.Sprite;
            }

            yield return StartCoroutine(_textCrawlDisplay.TextCrawl(dialog.Text));
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
