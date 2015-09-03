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
        public CutsceneDialog(string text, List<Sprite> sprites = null)
        {
            Text = text;
            Sprites = sprites;
        }

        public string Text { get; private set; }
        public List<Sprite> Sprites { get; private set; }
    }

    private List<CutsceneDialog> dialogs;
    private Image _image;

    // Use this for initialization
    void Start()
    {
        var censoredMode = Options.CensoredMode;


        _image = transform.FindChild("Image").GetComponent<Image>();

        var frameOne = new List<Sprite>
        {
            Resources.Load<Sprite>("Cards/Intro-01-1"),
            Resources.Load<Sprite>("Cards/Intro-01-2")
        }; 

        var frameTwo = new List<Sprite>{Resources.Load<Sprite>("Cards/Intro-02")};

        Sprite frameThreeSprite = censoredMode
            ? Resources.Load<Sprite>("Cards/Intro-03-Censored")
            : Resources.Load<Sprite>("Cards/Intro-03");

        var frameThree = new List<Sprite> { frameThreeSprite };
        var frameFour = new List<Sprite> 
        { 
            Resources.Load<Sprite>("Cards/Intro-04-1"), 
            Resources.Load<Sprite>("Cards/Intro-04-2")
        };

        dialogs = new List<CutsceneDialog>
        {
            new CutsceneDialog(
                "In a recent news story, it was reported that demand for security positions have been dwindling.", 
                frameOne),            

            new CutsceneDialog("Eric Eiselhauser has just finished listening this report in the nude, a Sunday tradition."),
            new CutsceneDialog("I can't believe this! Less security!", frameTwo),
            new CutsceneDialog("What if some weirdo showed up at the sports game! They could do whatever they pleased!"),
            new CutsceneDialog("..."),

            new CutsceneDialog("I know!", frameThree),

            new CutsceneDialog("I'll protest by running around the field!"),
            new CutsceneDialog("If enough people can see a normal guy running in view on all the cameras in the field, they'll understand the current security risks!"),
            new CutsceneDialog("They can hire more staff on before some weirdo tries something!"),
            new CutsceneDialog("Now let's get dressed and get going.", frameFour),
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
            if (dialog.Sprites != null)
            {
                if (_displaySpritesRoutine != null)
                {
                    StopCoroutine(_displaySpritesRoutine);
                    _displaySpritesRoutine = null;
                }
                _displaySpritesRoutine = DisplaySprites(dialog.Sprites);
                StartCoroutine(_displaySpritesRoutine);
            }

            yield return StartCoroutine(_textCrawlDisplay.TextCrawl(dialog.Text));
            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(3f);

        LoadMainScene();
    }

    private IEnumerator _displaySpritesRoutine;
    private IEnumerator DisplaySprites(List<Sprite> sprites)
    {
        while (true)
        {
            foreach (var sprite in sprites)
            {
                _image.sprite = sprite;
                yield return new WaitForSeconds(.5f);
            }
        }
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
