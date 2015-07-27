﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Xyz.Scripts;

public class Player : MonoBehaviour
{
    private Move _move;
    private ChaserContainer _chaserContainer;
    private StaminaCounter _staminaCounter;
    private MainSessionManager _mainSessionManager;

    private const float StaminaMax = 6f;
    private const float DancingMultiplier = 2f;
    private float _stamina;

    public void Start()
    {
        _move = GetComponent<Move>();
        _chaserContainer = transform.GetComponentInChildren<ChaserContainer>();
        _mainSessionManager = MainSessionManager.Instance;

        _stamina = StaminaMax;
        _staminaCounter = StaminaCounter.Instance;
    }

    public void Update()
    {
        var grabbedChaserCount = GetGrabbedChasers().Count;
        if (grabbedChaserCount > 0)
        {
            _stamina -= Time.smoothDeltaTime;
            if (_stamina < 0f)
            {
                _mainSessionManager.Failure();
                _move.Knockout();
            }
        }
        else if (_stamina < StaminaMax)
        {
            _stamina += Time.smoothDeltaTime * 2f;
        }
        else if (_stamina > StaminaMax)
        {
            _stamina = StaminaMax;
        }

        _staminaCounter.SetImagePercent(_stamina / StaminaMax);

        if (_currentBeacon != null)
        {
            var amount = GetBeaconDecrementAmount(DancingMultiplier, _move.IsDancing());
            _currentBeacon.DecrementCounter(amount);
        }
    }

    private Beacon _currentBeacon;
    void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParentAndExecuteIfExists<Beacon>(OnBeaconEnter);
        other.GetComponentAndExecuteIfExists<StadiumActiveArea>(OnStadiumActiveEnter);
        other.GetComponentInParentAndExecuteIfExists<Doorway>(OnDoorwayEnter);
    }

    void OnTriggerExit(Collider other)
    {
        other.GetComponentInParentAndExecuteIfExists<Beacon>(OnBeaconExit);
    }

    public void OnBeaconEnter(Beacon beacon)
    {
        _currentBeacon = beacon;
        _currentBeacon.SetState(Beacon.State.Capture);
    }

    public bool IsPreGame()
    {
        return _mainSessionManager.CurrentState == MainSessionManager.State.Pregame;
    }

    public void OnStadiumActiveEnter()
    {
        _mainSessionManager.PlayerHasEnteredArena();
    }

    public void OnDoorwayEnter(Doorway doorway)
    {
        StartCoroutine(OnDoorwayEnterCoroutine(doorway));
    }

    private IEnumerator OnDoorwayEnterCoroutine(Doorway doorway)
    {
        var enterTarget = doorway.EnterTarget;
        _move.LeaveArena(enterTarget.transform.position);
        yield return new WaitForSeconds(1f);
        DifficultyManager.Instance.LevelComplete();
    }

    public void OnBeaconExit(Beacon beacon)
    {
        beacon.SetState(Beacon.State.Idle);
        _currentBeacon = null;
    }

    public void AddChaser(Chaser chaser)
    {
        PlayerSpeechBubbleDisplay.Instance.DisplayText("If you cannot catch me how can we be safe?");

        _chaserContainer.AddChaser(chaser);
    }

    public List<Chaser> GetGrabbedChasers()
    {
        return _chaserContainer.GetChasers();
    }

    public void PushPlayer(Vector3 velocity)
    {
        _move.AddVelocity(velocity);
    }

    public float GetBeaconDecrementAmount(float multiplier, bool isDancing)
    {
        var amount = Time.smoothDeltaTime * 2f;
        if (_move.IsDancing())
        {
            amount *= DancingMultiplier;
        }

        return amount;
    }

    public void ResetAt(Vector3 position)
    {
        transform.position = position;
    }
}
