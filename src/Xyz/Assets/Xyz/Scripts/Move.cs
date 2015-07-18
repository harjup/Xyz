using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Managers;
using DG.Tweening;

public class Move : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Camera _camera;
    private CameraInput _cameraInput;
    private Player _player;
    private InputManager _inputManger;

    public float Acceleration = 640;
    public float Friction = 40f;
    public float MaxSpeed = 20f;

	void Start()
	{
	    _camera = Camera.main;
	    _cameraInput = _camera.GetComponent<CameraInput>();
	    _rigidbody = GetComponent<Rigidbody>();
	    _player = GetComponent<Player>();
        _inputManger = InputManager.Instance;
	}
	
	void Update()
	{
	    var grabbedChasers = _player.GetGrabbedChasers();
        var grabCount = grabbedChasers.Count;
	    var acceleration = Acceleration;
	    var maxSpeed = MaxSpeed;
	    if (grabCount > 0)
	    {
            acceleration = Acceleration / grabCount * 2;
	        maxSpeed = MaxSpeed / grabCount;


	        var deltaTotal = _inputManger.DeltaHorizontalAxis + _inputManger.DeltaVerticalAxis;

            grabbedChasers.First().ApplyWaggle(deltaTotal);
	    }

        var inputVector = _cameraInput.GetInputDirection(_camera);
        var playerForce = (inputVector * acceleration * Time.smoothDeltaTime);
	    var currentVelocity = _rigidbody.velocity.SetY(0);

        // Apply friction
        currentVelocity = ApplyFriction(currentVelocity, Friction, Time.smoothDeltaTime);

        if (currentVelocity.magnitude > maxSpeed)
	    {
            currentVelocity = currentVelocity.normalized * maxSpeed;
	    }
	    else
	    {
	        currentVelocity = currentVelocity + playerForce;
	    }

	    RotatePlayer(transform, currentVelocity);

        _rigidbody.velocity = currentVelocity;
	}

    public void AddVelocity(Vector3 velocity)
    {
        var difference = _rigidbody.velocity - velocity;
        _rigidbody.velocity += difference * 2f;
    }

    private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
    {
        return vector - vector.normalized * (friction * deltaTime);
    }

    private void RotatePlayer(Transform playerTransform, Vector3 direction)
    {
        //Rotate the player to face direction of movement only when input keys are pressed
        if (Math.Abs(_inputManger.RawHoritzontalAxis) >= .5f
            || Math.Abs(_inputManger.RawVerticalAxis) >= .5f)
        {       
            playerTransform.rotation = Quaternion.LookRotation(direction.SetY(0), Vector3.up);
        }
    }
}
