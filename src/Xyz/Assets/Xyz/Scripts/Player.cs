using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Xyz.Scripts;
using TreeEditor;

public class Player : MonoBehaviour
{
    private Move _move;
    //private Transform _chaserContainer;

    private ChaserContainer _chaserContainer;

    private InputManager _inputManager;

    public void Start()
    {
        _move = GetComponent<Move>();
        _chaserContainer = transform.GetComponentInChildren<ChaserContainer>();

        _inputManager = InputManager.Instance;
    }

    public void Update()
    {
        /*Debug.Log(string.Format(
            "{0}, {1}",
            _inputManager.DeltaHorizontalAxis,
            _inputManager.DeltaVerticalAxis));*/
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
