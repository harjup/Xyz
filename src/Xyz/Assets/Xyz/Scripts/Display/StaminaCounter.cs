using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaCounter : Singleton<StaminaCounter>
{
    private Image _staminaImage;
    private Image _staminaBackground;

    public void Start()
    {
        _staminaImage = transform.FindChild("Stamina-Green").GetComponent<Image>();
        _staminaBackground = transform.FindChild("Stamina-Red").GetComponent<Image>();
    }

    public void SetImagePercent(float val)
    {
        _staminaImage.fillAmount = val;

        if (val >= .99f)
        {
            if (_hideGraphic == null && _staminaImage.enabled)
            {
                _hideGraphic = HideGraphic();
                StartCoroutine(_hideGraphic);
            }
        }
        else
        {
            ShowGraphic();
        }

    }

    private IEnumerator _hideGraphic;
    private IEnumerator HideGraphic()
    {
        yield return new WaitForSeconds(.25f);
        _staminaImage.enabled = false;
        _staminaBackground.enabled = false;
        _hideGraphic = null;
        yield return new WaitForSeconds(.25f);
        PlayerSpeechBubbleDisplay.Instance.DisplayText(PlayerTaunts.GetRandomTaunt());
    }


    public void DisableGraphic()
    {
        _staminaImage.enabled = false;
        _staminaBackground.enabled = false;
    }

    private void ShowGraphic()
    {
        if (_hideGraphic != null)
        {
            StopCoroutine(_hideGraphic);
            _hideGraphic = null;
        }

        _staminaImage.enabled = true;
        _staminaBackground.enabled = true;
    }

}
