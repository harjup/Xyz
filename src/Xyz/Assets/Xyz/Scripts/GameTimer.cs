using UnityEngine;
using System.Collections;

public class GameTimer : Singleton<GameTimer>
{
    private enum State
    {
        Unknown,
        Running,
        Stopped,
        Paused
    }

    private State _state;
    private float _time;

    public void Start()
    {
        _time = 0f;
        _state = State.Stopped;
    }

    public void Update()
    {
        if (_state == State.Running)
        {
            _time += Time.smoothDeltaTime;
        }
    }

    public int GetSeconds()
    {
        return (int)_time;
    }

    public void StartTimer()
    {
        _time = 0f;
        _state = State.Running;
    }

    public void StopTimer()
    {
        _state = State.Stopped;
    }

    public bool IsRunning()
    {
        return _state == State.Running;
    }
}
