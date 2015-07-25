using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MainSessionManager : Singleton<MainSessionManager>
{
    public enum State
    {
        Unknown,
        Pregame,
        Streaking,
        Escape,
        Win,
        Lose
    }

    private State _state;
    private GameTimer _timer;

    List<int> _difficultIncreaseTimes = new List<int>{ 10, 30, 60, 90, 120};

    private int _beaconsRequired = 10;
    private int beaconCount;

    public void Start()
    {
        // Move player to random spawn point
        var player = FindObjectOfType<Player>();

        var spawn = 
            FindObjectsOfType<SpawnMarker>()
            .Where(m => m.IsFor(SpawnMarker.Type.Player))
            .ToList()
            .AsRandom()
            .First();

        player.ResetAt(spawn.transform.position);

        _timer = GameTimer.Instance;
        _beaconsRequired = beaconCount;
    }

    public void Update()
    {
        if (!IsOutInField() && _timer.IsRunning())
        {
            _timer.StopTimer();
        }

        int difficultyIncrease = _difficultIncreaseTimes.FirstOrDefault(d => d < _timer.GetSeconds());
        if (difficultyIncrease > 0)
        {
            _difficultIncreaseTimes.Remove(difficultyIncrease);
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty()
    {
        // Show a message:
        // Security as arrived!
        // Security has increased!
        // We have reached maximum security! GET OUT OF HERE!!!

        // Add 5 chasers to spawn queue
        ChaserSpawner.Instance.SpawnChasers(3);
    }

    public bool IsOutInField()
    {
        return _state == State.Streaking || _state == State.Escape;
    }

    public void PlayerHasEnteredArena()
    {
        if (IsOutInField())
        {
            return;
        }

        _timer.StartTimer();
        // Start timer
        _state = State.Streaking;
        // Initiate beacons (spawn some in)
        SpawnBeacons();
    }

    private void SpawnBeacons()
    {
        BeaconSpawner.Instance.SpawnBeacons(4);
    }
}
