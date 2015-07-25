using UnityEngine;
using System.Collections;

public class Beacon : MonoBehaviour 
{
    private const float CounterMax = 10f;
    private float _counter;

    private State _state;

    private BeaconCounter _beaconCounter;

    public enum State
    {
        Unknown,
        Idle,
        Capture,
        Done
    }

    public void Start()
    {
        _state = State.Idle;
        _counter = CounterMax;
        _beaconCounter = GetComponentInChildren<BeaconCounter>();
    }

    private IEnumerator _replaceRoutine;
    public void Update()
    {
        if (_state == State.Done)
        {
            if (_replaceRoutine == null)
            {
                _replaceRoutine = UsefulCoroutines.ExecuteAfterDelay(.5f, () =>
                {
                    Destroy(gameObject);
                    BeaconSpawner.Instance.SpawnBeacons();
                });

                StartCoroutine(_replaceRoutine);
            }

            return;
        }
        
        _beaconCounter.SetImagePercent(_counter / CounterMax);

        if (_state == State.Idle)
        {
            IncrementCounter(Time.smoothDeltaTime);
        }
    }

    public void IncrementCounter(float amount)
    {
        if (_counter < CounterMax)
        {
            _counter += amount;
        }
        else
        {
            _counter = CounterMax;
        }
    }

    public void SetState(State state)
    {
        if (_state == State.Done)
        {
            return;
        }

        _state = state;
    }

    public void DecrementCounter(float amount)
    {
        _counter -= amount;

        if (_counter <= 0)
        {
            _state = State.Done;
        }
    }
}
