using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    // This is specifically for the main gameplay area whoop
    private AudioSource _musicSource;
    private AudioSource _crowdSource;
    
    void Start()
    {
        _musicSource = transform.FindChild("MainMusic").GetComponent<AudioSource>();
        _crowdSource = transform.FindChild("Crowd-Idle").GetComponent<AudioSource>();
        _crowdSource.Play();
    }

    public void PlayMainTheme()
    {
        if (!_musicSource.isPlaying)
        {
            _musicSource.Play();
            _crowdSource.Stop();
        }
    }
}
