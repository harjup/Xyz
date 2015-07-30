using UnityEngine;
using System.Collections;

public class PlayNextSoundWhenFinished : MonoBehaviour
{
    public GameObject Target;
    private AudioSource _audioSource;
    private AudioSource _targetSource;

    private bool _fired = false;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _targetSource = Target.GetComponent<AudioSource>();

        _audioSource.Play();
    }

    void Update()
    {
        if (_fired)
        {
            return;
        }

        if (!_audioSource.isPlaying)
        {
            _targetSource.Play();
            _fired = true;
        }
    }
}
