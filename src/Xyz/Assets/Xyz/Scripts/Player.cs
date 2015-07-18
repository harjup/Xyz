using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Xyz.Scripts;
using TreeEditor;

public class Player : MonoBehaviour
{
    private Move _move;
    //private Transform _chaserContainer;

    private ChaserContainer _chaserContainer;

    public void Start()
    {
        _move = GetComponent<Move>();
        _chaserContainer = transform.GetComponentInChildren<ChaserContainer>();
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
