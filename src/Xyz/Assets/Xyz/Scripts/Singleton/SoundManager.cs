using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    // This is specifically for the main gameplay area whoop
    private AudioSource _musicSource;
    private AudioSource _crowdSource;
    private AudioSource _winSource;
    private AudioSource _lossSource;


    private AudioSource _pushedEffectSource;
    private AudioSource _grabbedEffectSource;
    private AudioSource _cameraDoneEffectSource;
    private AudioSource _cameraLoopEffectSource;

    private AudioSource _whistleEffectSource;
    


    void Start()
    {
        _musicSource = transform.FindChild("MainMusic").GetComponent<AudioSource>();
        _crowdSource = transform.FindChild("Crowd-Idle").GetComponent<AudioSource>();
        _winSource = transform.FindChild("Win-Track").GetComponent<AudioSource>();
        _lossSource = transform.FindChild("Loss-Track").GetComponent<AudioSource>();


        _pushedEffectSource = transform.FindChild("Pushed-Effect").GetComponent<AudioSource>();
        _grabbedEffectSource = transform.FindChild("Grabbed-Effect").GetComponent<AudioSource>();
        _cameraDoneEffectSource = transform.FindChild("Camera-Effect").GetComponent<AudioSource>();

        _cameraLoopEffectSource = transform.FindChild("Camera-Loop").GetComponent<AudioSource>();

        _whistleEffectSource = transform.FindChild("Whistle-Effect").GetComponent<AudioSource>();

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


    public void PlayGrabbedEffect()
    {
        _grabbedEffectSource.Play();
    }

    public void PlayPushedEffect()
    {
        _pushedEffectSource.Play();
    }

    public void PlayCameraDoneEffect()
    {
        _cameraDoneEffectSource.Play();
    }

    public void PlayCameraLoop()
    {
        if (!_cameraLoopEffectSource.isPlaying)
        {
            _cameraLoopEffectSource.Play();
        }
    }

    public void StopCameraLoop()
    {
        _cameraLoopEffectSource.Stop();
    }

    public void PlayWhistleEffect()
    {
        _whistleEffectSource.Play();
    }
}
