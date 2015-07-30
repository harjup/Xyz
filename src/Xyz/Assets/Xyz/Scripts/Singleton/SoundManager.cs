using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    // This is specifically for the main gameplay area whoop
    private AudioSource _musicSource;
    private AudioSource _crowdSource;
    private AudioSource _winSource;
    private AudioSource _lossSource;
    
    void Start()
    {
        _musicSource = transform.FindChild("MainMusic").GetComponent<AudioSource>();
        _crowdSource = transform.FindChild("Crowd-Idle").GetComponent<AudioSource>();
        _winSource = transform.FindChild("Win-Track").GetComponent<AudioSource>();
        _lossSource = transform.FindChild("Loss-Track").GetComponent<AudioSource>();

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

    public void PlayWinTrack()
    {
        if (!_winSource.isPlaying)
        {
            _musicSource.Stop();
            _winSource.Play();
        }
    }

    public void PlayLossTrack()
    {
        if (!_winSource.isPlaying)
        {
            _musicSource.Stop();
            _lossSource.Play();
        }
    }

}
