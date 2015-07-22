using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Xyz.Scripts;
using TreeEditor;

public class Player : MonoBehaviour
{
    private Move _move;
    private ChaserContainer _chaserContainer;
    private InputManager _inputManager;
    private StaminaCounter _staminaCounter;

    private const float StaminaMax = 3f;
    private float _stamina;

    public void Start()
    {
        _move = GetComponent<Move>();
        _chaserContainer = transform.GetComponentInChildren<ChaserContainer>();

        _inputManager = InputManager.Instance;

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
    }
    
    public void AddChaser(Chaser chaser)
    {
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
}
