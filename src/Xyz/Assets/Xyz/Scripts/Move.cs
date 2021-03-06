﻿using System;
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
    private CameraMove _cameraMove;
    private Player _player;
    private InputManager _inputManger;
    private PlayerMesh _mesh;

    private Animator _animator;

    // This is here so we can trade a push the the player's input for the current frame
    // I am paranoid about async problems if we're assigning rigidbody velocity in multiple spots
    private Vector3? _outstandingPush;

    public enum State
    {
        Run, 
        Dance,
        Knockout,
        Bumped,
        LeavingArena
    }

    [SerializeField]
    private State _state;

    public float Acceleration = 640;
    public float Friction = 40f;
    public float MaxSpeed = 20f;
    public float SpeedLostFromGrapple = 2f;
    public float inputMultiplier = 1f;

    private bool _struggle = false;


	void Start()
	{
	    _camera = Camera.main;
	    _cameraMove = _camera.GetComponent<CameraMove>();
	    _cameraInput = _camera.GetComponent<CameraInput>();
	    _rigidbody = GetComponent<Rigidbody>();
	    _player = GetComponent<Player>();
        _inputManger = InputManager.Instance;

	    _mesh = transform.GetComponentInChildren<PlayerMesh>();

	    _state = State.Run;
	    _outstandingPush = null;

        _animator = gameObject.GetComponentInChildren<Animator>();
	}
	
	void Update()
	{
	    if (_state == State.Knockout || _state == State.LeavingArena)
	    {
	        return;
	    }

	    RunUpdate();
	}

    public void RunUpdate()
    {
        var acceleration = Acceleration;
        var maxSpeed = MaxSpeed;

        var grabbedChasers = _player.GetGrabbedChasers();
        var grabCount = grabbedChasers.Count;


        _struggle = grabCount > 0;
        if (_struggle)
        {
            acceleration = Acceleration/grabCount;
            maxSpeed = MaxSpeed - SpeedLostFromGrapple;

            var deltaTotal = _inputManger.DeltaHorizontalAxis + _inputManger.DeltaVerticalAxis;

            grabbedChasers.First().ApplyWaggle(deltaTotal);
        }

        var inputVector = _cameraInput.GetInputDirection(_camera);
        var playerForce = inputVector * acceleration * Time.smoothDeltaTime;
        var currentVelocity = _rigidbody.velocity.SetY(0);

        currentVelocity = ApplyFriction(currentVelocity, Friction, maxSpeed, Time.smoothDeltaTime);
        currentVelocity = ApplyInputForce(currentVelocity, playerForce, maxSpeed);

        RotatePlayer(transform, currentVelocity);

        _rigidbody.velocity = currentVelocity.SetY(-10f);
        if (_outstandingPush.HasValue)
        {
            _rigidbody.velocity = _outstandingPush.Value;
            _outstandingPush = null;
        }

        

        if (currentVelocity.sqrMagnitude > 1f)
        {
            Run();
        }
        else
        {
            if (_player.IsPreGame())
            {
                Idle();
            }
            else if (_struggle)
            {
                Struggle();
            }
            else
            {
                Dance();
            }
        }
    }

    private Sequence _currentSequence;
    public void ShakeCamera()
    {
        if (_currentSequence != null && _currentSequence.IsPlaying())
        {
            _currentSequence.Complete();
        }

        _currentSequence = DOTween.Sequence();

        _currentSequence
            .Append(_camera.DOShakePosition(.25f))
            .OnComplete(() => { _cameraMove.ResetPosition(); })
            .Play();
    }

    public void AddVelocity(Vector3 velocity, int maximum)
    {
        var difference = _rigidbody.velocity - velocity;
        _rigidbody.velocity += difference;
        if (_rigidbody.velocity.magnitude > maximum)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * maximum;
        }

    }

    private Vector3 ApplyFriction(Vector3 velocity, float friction, float maxSpeed, float deltaTime)
    {
        if (velocity.magnitude > maxSpeed)
        {
            friction *= 2f;
        }

        return velocity - velocity.normalized * (friction * deltaTime);
    }

    public Vector3 ApplyInputForce(Vector3 velocity, Vector3 force, float maxSpeed)
    {
        var newVelocity = velocity + force;

        if (velocity.magnitude > maxSpeed)
        {
            force = force / 8f;

            // We need to eliminate all force in the direction of velocity when applying our own force

            var normal = velocity.normalized;
            var normalV2 = new Vector2(normal.x, normal.z);
            var forceV2 = new Vector2(force.x, force.z);

            var v1 = Vector2.Dot(normalV2, forceV2);
            if (v1 < 0)
            {
                Debug.DrawRay(transform.position, force, Color.cyan, 3f);
                return velocity + force;
            }
            var v2 = forceV2 - (normalV2 * v1);

            Debug.DrawRay(transform.position, new Vector3(v2.x, 0 ,v2.y), Color.cyan, 3f);

            return velocity + new Vector3(v2.x, 0 ,v2.y);
        }

        
        return ApplySpeedCap(newVelocity, maxSpeed);
        // If velocity + force <= maxSpeed return it
        // If velocity < maxSpeed && velocity
    }

    public Vector3 ApplySpeedCap(Vector3 velocity, float maxSpeed)
    {
        if (velocity.magnitude > maxSpeed)
        {
            return velocity.normalized * (maxSpeed);
        }
        
        return velocity;
    }

    private void RotatePlayer(Transform playerTransform, Vector3 direction)
    {
        //Rotate the player to face direction of movement only when input keys are pressed
        if (Math.Abs(_inputManger.RawHoritzontalAxis) >= .1f
            || Math.Abs(_inputManger.RawVerticalAxis) >= .1f)
        {       
            playerTransform.rotation = Quaternion.LookRotation(direction.SetY(0), Vector3.up);
        }
    }

    public void Run()
    {
        _state = State.Run;
        _animator.Play("Walk");
    }

    public void Dance()
    {
        if (_state != State.Dance)
        {
            _animator.Play("Dance");
            _state = State.Dance;
        }
    }

    public void Idle()
    {
        _state = State.Run;
        _animator.Play("Idle");
    }

    public void Struggle()
    {
        _state = State.Run;
        _animator.Play("Struggle");
    }

    public void Knockout()
    {
        _animator.Play("Fall");
        _rigidbody.velocity = Vector3.zero;
        _state = State.Knockout;

        //_rigidbody.DORotate(new Vector3(0, 180, 90), .5f, RotateMode.Fast);
    }

    public void LeaveArena(Vector3 target)
    {
        _state = State.LeavingArena;
        var untouchableLayer = LayerMask.NameToLayer("Untouchable");
        gameObject.SetLayerRecursively(untouchableLayer);

        var direction = (target - transform.position).normalized;
        _rigidbody.velocity = direction*MaxSpeed;

        _cameraMove.Pause = true;
    }

    public bool IsDancing()
    {
        return _state == State.Dance;
    }

    public bool IsKnockedOut()
    {
        return _state == State.Knockout;
    }

    public void Push(Vector3 force)
    {
        // Reduce acceleration effect for .25 seconds
        // Set velocity to push force
        if (_pushRoutine != null)
        {
            StopCoroutine(_pushRoutine);
        }
        _pushRoutine = PushRoutine(force);
        StartCoroutine(_pushRoutine);
    }

    private IEnumerator _pushRoutine;
    IEnumerator PushRoutine(Vector3 force)
    {
        _outstandingPush = force;
        ShakeCamera();
        yield return null;
    }
}
