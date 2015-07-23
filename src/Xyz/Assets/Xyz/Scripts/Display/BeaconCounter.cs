using UnityEngine;
using UnityEngine.UI;

public class BeaconCounter : MonoBehaviour 
{
    private Image _staminaImage;

    public void Start()
    {
        _staminaImage = transform.FindChild("Default").GetComponent<Image>();
    }

    public void SetImagePercent(float val)
    {
        _staminaImage.fillAmount = val;
    }
}
