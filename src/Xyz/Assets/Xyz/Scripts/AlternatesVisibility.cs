using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlternatesVisibility : MonoBehaviour
{
    public float ToggleRate = 1f;
    public bool Paused = false;

    private Renderer _renderer;

    
    public void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        gameObject.GetComponent<Renderer>().enabled = true;
        StartCoroutine(ToggleVisibility());
    }

    // This only gets called once in start. 
    // If our object is set as disabled and enabled again, then visibility won't get toggled.
    private IEnumerator ToggleVisibility()
    {
        _renderer.enabled = !_renderer.enabled;

        while (Paused)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(ToggleRate);

        StartCoroutine(ToggleVisibility());
    }

}
