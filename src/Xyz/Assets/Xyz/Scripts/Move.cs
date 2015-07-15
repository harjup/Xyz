using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using DG.Tweening;

public class Move : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Camera _camera;

    public float Acceleration = 640;
    public float Friction = 40f;
    public float MaxSpeed = 20f;

	void Start()
	{
	    _camera = Camera.main;
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update()
	{
	    var inputVector = GetInputDirection(_camera);
        var playerForce = (inputVector * Acceleration * Time.smoothDeltaTime);
	    var currentVelocity = _rigidbody.velocity.SetY(0);

        // Apply friction
        currentVelocity = ApplyFriction(currentVelocity, Friction, Time.smoothDeltaTime);

        if (currentVelocity.magnitude > MaxSpeed)
	    {
            currentVelocity = currentVelocity.normalized * MaxSpeed;
	    }
	    else
	    {
	        currentVelocity = currentVelocity + playerForce;
	    }

	    RotatePlayer(transform, currentVelocity);

        _rigidbody.velocity = currentVelocity;
	}

    private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
    {
        return vector - vector.normalized * (friction * deltaTime);
    }

    private Vector3 GetInputDirection(Camera camera)
    {
        var forward = Vector3.forward;
        //Create the reference axis based on the camera rotation, ignoring y rotation
        //We're getting the main camera, which should be the one that's enabled. It's null if disabled so don't do anything if so
        if (camera != null)
        {
            forward = camera.transform.TransformDirection(Vector3.forward).SetY(0).normalized;
        }

        var right = new Vector3(forward.z, 0.0f, -forward.x);

        //Set the player's walk direction
        Vector3 walkVector = (
            InputManager.Instance.HoritzontalAxis * right
            + InputManager.Instance.VerticalAxis * forward
            );

        //prevent the player from moving faster when walking diagonally
        if (walkVector.sqrMagnitude > 1f)
            walkVector = walkVector.normalized;

        return walkVector;
    }

    private void RotatePlayer(Transform playerTransform, Vector3 direction)
    {
        //Rotate the player to face direction of movement only when input keys are pressed
        if (Math.Abs(InputManager.Instance.RawHoritzontalAxis) >= .99f
            || Math.Abs(InputManager.Instance.RawVerticalAxis) >= .99f)
        {       
            playerTransform.rotation = Quaternion.LookRotation(direction.SetY(0), Vector3.up);
        }
    }
}
