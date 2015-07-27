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
    private CameraMove _cameraMove;
    private Player _player;
    private InputManager _inputManger;
    private PlayerMesh _mesh;

    public enum State
    {
        Run, 
        Dance,
        Knockout,
        LeavingArena
    }

    [SerializeField]
    private State _state;

    public float Acceleration = 640;
    public float Friction = 40f;
    public float MaxSpeed = 20f;

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

        if (grabCount > 0)
        {
            acceleration = Acceleration/grabCount*2;
            maxSpeed = MaxSpeed/grabCount;

            var deltaTotal = _inputManger.DeltaHorizontalAxis + _inputManger.DeltaVerticalAxis;

            grabbedChasers.First().ApplyWaggle(deltaTotal);
        }

        var inputVector = _cameraInput.GetInputDirection(_camera);
        var playerForce = inputVector * acceleration * Time.smoothDeltaTime;
        var currentVelocity = _rigidbody.velocity.SetY(0);

        currentVelocity = ApplyFriction(currentVelocity, Friction, Time.smoothDeltaTime);
        currentVelocity = ApplyInputForce(currentVelocity, playerForce);
        currentVelocity = ApplySpeedCap(currentVelocity, maxSpeed);

        RotatePlayer(transform, currentVelocity);

        _rigidbody.velocity = currentVelocity.SetY(-10f);

        if (currentVelocity.sqrMagnitude > 1f || grabCount > 0 || _player.IsPreGame())
        {
            Run();
        }
        else
        {
            Dance();
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

    public void AddVelocity(Vector3 velocity)
    {
        var difference = _rigidbody.velocity - velocity;
        _rigidbody.velocity += difference * 2f;
    }

    private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
    {
        return vector - vector.normalized * (friction * deltaTime);
    }

    public Vector3 ApplyInputForce(Vector3 velocity, Vector3 force)
    {  
        return velocity + force;    
    }

    public Vector3 ApplySpeedCap(Vector3 velocity, float maxSpeed)
    {
        if (velocity.magnitude > maxSpeed)
        {
            return velocity.normalized * maxSpeed;
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
        _mesh.StopDancing();
        // mess with animations
    }

    private Tweener _danceTweener;
    public void Dance()
    {
        if (_state != State.Dance)
        {
            _state = State.Dance;
            // play animations
            _mesh.DanceTween();
        }
    }

    public void Knockout()
    {
        _rigidbody.velocity = Vector3.zero;
        _state = State.Knockout;
        _rigidbody.DORotate(new Vector3(0, 180, 90), .5f, RotateMode.Fast);
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
}
