using UnityEngine;
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

    public State CurrentState { get { return _state; } }

    List<DifficultyEvent> _difficultIncreaseTimes = new List<DifficultyEvent>
    {
        new DifficultyEvent(10, "Security has arrived!", DifficultyEvent.Type.AddChasers),
        new DifficultyEvent(30, "Security has increased.", DifficultyEvent.Type.AddChasers),
        new DifficultyEvent(60, "Security has increased again.", DifficultyEvent.Type.AddChasers),
        new DifficultyEvent(90, "We have reached max security!", DifficultyEvent.Type.AddChasers)
    };

    private MessageManager _messageManager;
    private DoorwayManager _doorwayManager;
    private int _beaconsRequired = 5;
    private int _chasersPerWave = 3;
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
        _messageManager = MessageManager.Instance;
        _doorwayManager = DoorwayManager.Instance;
    }

    public void Update()
    {
        if (!IsOutInField() && _timer.IsRunning())
        {
            _timer.StopTimer();
        }

        var chaserSpawnEvent = _difficultIncreaseTimes
            .Where(d => !d.Fired)
            .Where(d => d.EventType == DifficultyEvent.Type.AddChasers)
            .FirstOrDefault(d => d.Time < _timer.GetSeconds());

        if (chaserSpawnEvent != null)
        {
            IncreaseDifficulty(chaserSpawnEvent);
        }
    }

    private void IncreaseDifficulty(DifficultyEvent difficultyEvent)
    {
        difficultyEvent.Fired = true;
        
        if (difficultyEvent.EventType == DifficultyEvent.Type.AddChasers)
        {
            ChaserSpawner.Instance.SpawnChasers(_chasersPerWave);
            _messageManager.ShowMessage(difficultyEvent.Message);
        }
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

        _messageManager.ShowMessage("Show the world your cause!");

        _timer.StartTimer();
        // Start timer
        _state = State.Streaking;
        // Initiate beacons (spawn some in)
        SpawnInitialBeacons();
    }

    private void SpawnInitialBeacons()
    {
        BeaconSpawner.Instance.SpawnBeacons(4);
    }


    private bool _newBeaconsNeeded;

    public bool CaptureBeacon()
    {    
        var beaconsInField = FindObjectsOfType<Beacon>().Count();
        var totalBeacons = beaconCount + beaconsInField;
        _newBeaconsNeeded = totalBeacons < _beaconsRequired;

        beaconCount++;
        if (beaconCount >= _beaconsRequired)
        {
            _state = State.Escape;
            _messageManager.ShowMessage("Get out of here!!!");
            _doorwayManager.EnableAllExits();
        } 

        return _newBeaconsNeeded;
    }

    public bool IsNewBeaconNeeded()
    {
        return _newBeaconsNeeded;
    }

    public void SetDifficulty(int clearedLevels)
    {
        _beaconsRequired = 5 + (clearedLevels * 3);
        //_chasersPerWave = 3 + clearedLevels;
    }
}
