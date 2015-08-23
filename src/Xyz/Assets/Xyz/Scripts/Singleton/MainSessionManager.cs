using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Xyz.Scripts;
using UnityEngine.VR;

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

    private List<DifficultyEvent> _difficultIncreaseTimes;

    private MessageManager _messageManager;
    private DoorwayManager _doorwayManager;

    public int BeaconsRequired { get { return _beaconsRequired; }}
    public int BeaconCount { get { return beaconCount; }}
    

    private int _beaconsRequired = 5;
    private int _chasersPerWave = 2;
    private int _pushersPerWave = 0;
    private int beaconCount;

    public void Start()
    {
        var scheduleGenerator = new SpawnScheduleGenerator();
        _difficultIncreaseTimes = scheduleGenerator.GetSteppedSchedule();

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

        _state = State.Pregame;
    }

    public void Update()
    {
        if (!IsOutInField() && _timer.IsRunning())
        {
            _timer.StopTimer();
        }

        var chaserSpawnEvent = _difficultIncreaseTimes
            .Where(d => !d.Fired)
            .Where(d => d.EventType == DifficultyEvent.Type.AddChasers 
                || d.EventType == DifficultyEvent.Type.AddPushers)
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
            if (difficultyEvent.Time >= 60)
            {
                ChaserSpawner.Instance.SpawnChasers(_chasersPerWave / 2);
            }
            else
            {
                ChaserSpawner.Instance.SpawnChasers(_chasersPerWave);
            }

            SoundManager.Instance.PlayWhistleEffect();
            _messageManager.ShowMessage(difficultyEvent.Message);
        }

        if (difficultyEvent.EventType == DifficultyEvent.Type.AddPushers)
        {
            if (difficultyEvent.Time == 30)
            {
                // Let's have half the amount come in the second pusher wave
                ChaserSpawner.Instance.SpawnPushers(_pushersPerWave/2);
            }
            else
            {
                ChaserSpawner.Instance.SpawnPushers(_pushersPerWave);
            }
            
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

        _messageManager.ShowMessage("Show the cameras your cause!");
        SoundManager.Instance.PlayMainTheme();


        _timer.StartTimer();
        // Start timer
        _state = State.Streaking;
        // Initiate beacons (spawn some in)
        SpawnInitialBeacons();
    }

    private void SpawnInitialBeacons()
    {
        BeaconSpawner.Instance.SpawnBeacons(3);
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

            StartCoroutine(ShowheadForExitInstruction());
        } 

        return _newBeaconsNeeded;
    }

    private IEnumerator ShowheadForExitInstruction()
    {
        yield return new WaitForSeconds(5f);
        MainCanvasManager.Instance.ShowExitDisplay();
    }


    public void Failure()
    {
        _timer.StopTimer();
        MainCanvasManager.Instance.FailFailureDisplay();
    }

    public bool IsNewBeaconNeeded()
    {
        return _newBeaconsNeeded;
    }

    public void SetRequiredBeacons(int amount)
    {
        _beaconsRequired = amount;
        
    }

    public void SetChasersPerWave(int amount)
    {
        _chasersPerWave = amount;
    }

    public void SetPushersPerWave(int amount)
    {
        _pushersPerWave = amount;
    }

    public IEnumerator LevelComplete()
    {
        _timer.StopTimer();
        yield return new WaitForSeconds(3f);
        MainCanvasManager.Instance.ShowWinDisplay();
        yield return new WaitForSeconds(2f);
        MainCanvasManager.Instance.HideAll();
        yield return new WaitForSeconds(1.5f);
        DifficultyManager.Instance.LevelComplete();
    }
}
