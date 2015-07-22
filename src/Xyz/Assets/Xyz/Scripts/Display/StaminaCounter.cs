using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaCounter : Singleton<StaminaCounter>
{
    private Image _staminaImage;

    public void Start()
    {
        _staminaImage = transform.FindChild("Stamina-Green").GetComponent<Image>();
    }

    public void SetImagePercent(float val)
    {
        _staminaImage.fillAmount = val;
    }

}
