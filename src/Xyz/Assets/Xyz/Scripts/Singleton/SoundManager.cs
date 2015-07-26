using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    // This is specifically for the main gameplay area whoop
    private AudioSource _musicSource;
    
    void Start()
    {
        _musicSource = transform.FindChild("MainMusic").GetComponent<AudioSource>();
    }

    public void PlayMainTheme()
    {
        if (!_musicSource.isPlaying)
        {
            _musicSource.Play();
        }
    }
}
