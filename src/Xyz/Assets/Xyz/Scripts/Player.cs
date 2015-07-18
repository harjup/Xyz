using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Xyz.Scripts;

public class Player : MonoBehaviour
{
    private List<Chaser> Chasers;
    private Move _move;
    private Transform _playerTransform;

    public void Start()
    {
        _move = GetComponent<Move>();
        _playerTransform = transform.GetChild(0);
    }

    public void AddChaser(Chaser chaser)
    {
        chaser.transform.parent = _playerTransform;
        var rigidbody = chaser.GetComponent<Rigidbody>();
        var collider = chaser.GetComponent<Collider>();

        // TODO Wrap the following lines in a function on chaser.cs
        var velocity = rigidbody.velocity;

        rigidbody.velocity = Vector3.zero;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Change chaser tags in collision matrix so they won't collide with player
        //
        
        _move.AddVelocity(velocity);
    }
}
