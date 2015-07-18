using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Xyz.Scripts
{
    public class Chaser : MonoBehaviour
    {
        private GameObject _player;
        private Rigidbody _rigidbody;
        private Collider _collider;

        public float Acceleration = 640;
        public float Friction = 40f;
        public float MaxSpeed = 10f;

        public enum State
        {
            Unknown,
            Run,
            Charge,
            Grabbed
        }

        public State _state = State.Unknown;


        public void Start()
        {
            _state = State.Run;

            _player = FindObjectOfType<Move>().gameObject;
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private IEnumerator _chargeInDirectionRoutine;
        void Update()
        {
            if (_state == State.Grabbed)
            {
                return;
            }

            var playerPosition = GetRelativePlayerPosition();
            const float chargeDistance = 10f;
            var shouldCharge = playerPosition.sqrMagnitude < Mathf.Pow(chargeDistance, 2f);

            if (shouldCharge && _state != State.Charge)
            {
                _state = State.Charge;
            }

            if (_state == State.Run)
            {    
                FollowPlayer(playerPosition);
            }

            if (_state == State.Charge && _chargeInDirectionRoutine == null)
            {
                _chargeInDirectionRoutine = ChargeInDirection(playerPosition);
                StartCoroutine(_chargeInDirectionRoutine);
            }
        }

        public Vector3 GetRelativePlayerPosition()
        {
            return _player.transform.position - transform.position;
        }

        public void FollowPlayer(Vector3 movementDirection)
        {
            var inputVector = (movementDirection).normalized;
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

            _rigidbody.velocity = currentVelocity;
        }


        private const float MaxChargeSpeed = 30f;
        public IEnumerator ChargeInDirection(Vector3 targetDirection)
        {
            var inputVector = (targetDirection).normalized;
            var playerForce = (inputVector * Acceleration * 2f  * Time.smoothDeltaTime);
            var currentVelocity = _rigidbody.velocity.SetY(0);

            // Apply friction
            currentVelocity = ApplyFriction(currentVelocity, Friction, Time.smoothDeltaTime);

            if (currentVelocity.magnitude > MaxChargeSpeed)
            {
                currentVelocity = currentVelocity.normalized * MaxChargeSpeed;
            }
            else
            {
                currentVelocity = currentVelocity + playerForce;
            }

            _rigidbody.velocity = currentVelocity;

            yield return new WaitForSeconds(.5f);

            _chargeInDirectionRoutine = null;

            if (_state != State.Grabbed)
            {
                _state = State.Run;
            }
        }

        private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
        {
            return vector - vector.normalized * (friction * deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                _state = State.Grabbed;
                player.AddChaser(this);
            }
        }
    }
}
