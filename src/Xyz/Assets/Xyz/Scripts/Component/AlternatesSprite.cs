using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AlternatesSprite : MonoBehaviour
{
    private Image _image;
    public List<Sprite> _sprites;
    // Use this for initialization
    void Start()
    {
        _image = GetComponent<Image>();
        StartCoroutine(DisplaySprites(_sprites));
    }

    private IEnumerator DisplaySprites(List<Sprite> sprites)
    {
        while (true)
        {
            foreach (var sprite in sprites)
            {
                _image.sprite = sprite;
                yield return new WaitForSeconds(.25f);
            }
        }
    }
}
