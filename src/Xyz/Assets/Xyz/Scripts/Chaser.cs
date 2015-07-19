using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using UnityEditor;
using UnityEngine;

namespace Assets.Xyz.Scripts
{
    public class Chaser : MonoBehaviour
    {
        public float Acceleration = 640;
        public float Friction = 40f;
        public float MaxSpeed = 10f;
        public float GrabStaminaMax = 20f;

        private float _grabStamina;

        private GameObject _player;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private CameraInput _cameraInput;

        private int _grabLayer;
        private int _defaultLayer;

        public enum State
        {
            Unknown,
            Run,
            Charge,
            Grabbed,
            Fall
        }

        public State _state = State.Unknown;


        public void Start()
        {
            _state = State.Run;

            _player = FindObjectOfType<Move>().gameObject;
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            _grabLayer = LayerMask.NameToLayer("Chaser-Grab");
            _defaultLayer = gameObject.layer;

            _grabStamina = GrabStaminaMax;

            _cameraInput = Camera.main.GetComponent<CameraInput>();
        }

        private IEnumerator _chargeInDirectionRoutine;
        private IEnumerator _fallWaitRoutine ;
        void Update()
        {
            if (_state == State.Grabbed || _state == State.Fall)
            {
                return;
            }

            var playerPosition = GetRelativePlayerPosition();
            const float chargeDistance = 10f;

            var shouldCharge = playerPosition.sqrMagnitude < Mathf.Pow(chargeDistance, 2f);
            shouldCharge = shouldCharge && _state != State.Charge;

            if (shouldCharge)
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

        private IEnumerator _fallRecoveryRoutine;
        public IEnumerator FallRecovery(Vector3 fallDirection)
        {
            _state = State.Fall;
            transform.parent = null;

            var inputDirection = fallDirection.normalized;

            _rigidbody.constraints =
                RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezeRotation;

            _rigidbody.velocity = inputDirection * 40f;

            yield return new WaitForSeconds(.5f);

            _rigidbody.velocity = Vector3.zero;;

            yield return new WaitForSeconds(2f);

            _fallRecoveryRoutine = null;
            gameObject.layer = _defaultLayer;
            _state = State.Run;
        }

        private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
        {
            return vector - vector.normalized * (friction * deltaTime);
        }

        public void GrabPlayer(Player player)
        {
            _state = State.Grabbed;
            player.AddChaser(this);
            player.PushPlayer(_rigidbody.velocity);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            gameObject.layer = _grabLayer;
        }

        void OnCollisionEnter(Collision collision)
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                GrabPlayer(player);
            }


            if (_fallRecoveryRoutine != null)
            {
                return;
            }

            // TODO: Need to check if hitting wall at a vaugely 90 degree angle. Steal from wizcombat probably
            var chaser = collision.gameObject.GetComponent<Chaser>();
            if (chaser != null)
            {
                _fallRecoveryRoutine = FallRecovery(_rigidbody.velocity * -1);
                StartCoroutine(_fallRecoveryRoutine);
            }

            // TODO: Need to check if both are charging in the general direction of each other
            var isWall = collision.gameObject.tag == "Barrier";
            if (isWall)
            {
                _fallRecoveryRoutine = FallRecovery(_rigidbody.velocity*-1);
                StartCoroutine(_fallRecoveryRoutine);
            }
        }

        public void ApplyWaggle(float deltaTotal)
        {
            _grabStamina -= deltaTotal;
            if (_grabStamina < 0f)
            {
                var inputDirection = _cameraInput.GetInputDirection();
                _fallRecoveryRoutine = FallRecovery(inputDirection);
                StartCoroutine(_fallRecoveryRoutine);
                _grabStamina = GrabStaminaMax;
            }
        }
    }
}
