using UnityEngine;
using System.Collections;

public class PlayMovieTexture : MonoBehaviour
{
    void Start()
    {
        var movieTexture = GetComponent<Renderer>().material.mainTexture as MovieTexture;

        movieTexture.loop = true;
        movieTexture.Play();
    }
}
