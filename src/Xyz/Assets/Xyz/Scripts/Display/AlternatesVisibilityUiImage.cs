using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlternatesVisibilityUiImage : MonoBehaviour
{
    public float ToggleRate = 1f;
    public bool Paused = false;

    private Image _renderer;

    private IEnumerator _visibilityRoutine;
    public void Start()
    {
        _renderer = gameObject.GetComponent<Image>();
        gameObject.GetComponent<Image>().enabled = true;
    }

    public void Update()
    {
        if (_visibilityRoutine == null)
        {
            _visibilityRoutine = ToggleVisibility();
            StartCoroutine(ToggleVisibility());
        }
    }

    // This only gets called once in start. 
    // If our object is set as disabled and enabled again, then visibility won't get toggled.
    // If it starts as disabled we're fine
    private IEnumerator ToggleVisibility()
    {
        _renderer.enabled = !_renderer.enabled;

        while (Paused)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(ToggleRate);

        _visibilityRoutine = null;
    }
}
