using UnityEngine;
using System.Collections;

public class Beacon : MonoBehaviour 
{
    private const float CounterMax = 10f;
    private float _counter;

    private State _state;

    private BeaconCounter _beaconCounter;
    private Camera _localCamera;

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
        _localCamera = GetComponentInChildren<Camera>();
        _localCamera.enabled = false;
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
                    var mainSessionManager = MainSessionManager.Instance;
                    var newBeaconNeeded = mainSessionManager.CaptureBeacon();
                    if (newBeaconNeeded)
                    {
                        BeaconSpawner.Instance.SpawnBeacons();
                    }

                    Destroy(gameObject);
                });

                StartCoroutine(_replaceRoutine);
            }

            return;
        }
        
        _beaconCounter.SetImagePercent(_counter / CounterMax);

        if (_state == State.Idle)
        {
            IncrementCounter(Time.smoothDeltaTime / 2f);
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
        _localCamera.enabled = state == State.Capture;
        
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
