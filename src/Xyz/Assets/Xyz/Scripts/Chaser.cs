using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Xyz.Scripts
{
    public class Chaser : MonoBehaviour
    {
        public float Acceleration = 640;
        public float Friction = 40f;
        public float MaxSpeed = 10f;
        public float GrabStaminaMax = 20f;

        private float _grabStamina;

        private GameObject _playerMove;
        private Rigidbody _rigidbody;
        private CameraInput _cameraInput;
        private GameObject _collider;

        private int _grabLayer;
        private int _defaultLayer;
        private int _stunnedLayer;

        private GameObject _defaultMesh;
        //private GameObject _stunnedMesh;
        private Animator _animator;

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
            MaxSpeed = Random.Range(8, 14);

            _state = State.Run;

            _playerMove = FindObjectOfType<Move>().gameObject;
            _rigidbody = GetComponent<Rigidbody>();

            _defaultLayer = gameObject.layer;
            _grabLayer = LayerMask.NameToLayer("Chaser-Grab");
            _stunnedLayer = LayerMask.NameToLayer("Chaser-Stun");
            
            _grabStamina = GrabStaminaMax;

            _cameraInput = Camera.main.GetComponent<CameraInput>();

            _animator = GetComponentInChildren<Animator>();
            _animator.Play("Walk");
            _defaultMesh = transform.FindChild("Mesh").gameObject;
            //_stunnedMesh = transform.FindChild("Mesh-Stunned").gameObject;

            //_defaultMesh.SetActive(true);
            //_stunnedMesh.SetActive(false);
        }

        private IEnumerator _chargeInDirectionRoutine;
        void FixedUpdate()
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
                LookAtPlayer(playerPosition);
            }

            if (_state == State.Charge && _chargeInDirectionRoutine == null)
            {
                _chargeInDirectionRoutine = ChargeInDirection(playerPosition);
                StartCoroutine(_chargeInDirectionRoutine);
            }
        }

        public Vector3 GetRelativePlayerPosition()
        {
            return _playerMove.transform.position - transform.position;
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

        public void LookAtPlayer(Vector3 direction)
        {
            _defaultMesh.transform.rotation = Quaternion.LookRotation(direction.SetY(0), Vector3.up);
        }


        private const float MaxChargeSpeed = 30f;
        public IEnumerator ChargeInDirection(Vector3 targetDirection)
        {
            _animator.Play("Dive");
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
                _animator.Play("Walk");
                _state = State.Run;
            }
        }

        private IEnumerator _fallRecoveryRoutine;
        public IEnumerator FallRecovery(Vector3 fallDirection)
        {
            _animator.Play("FellDown");
            //_defaultMesh.SetActive(false);
            //_stunnedMesh.SetActive(true);

            _state = State.Fall;
           
            gameObject.SetLayerRecursively(_stunnedLayer);

            var inputDirection = fallDirection.normalized;

            _rigidbody.constraints =
                RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezeRotation;

            _rigidbody.velocity = inputDirection * 40f;

            yield return new WaitForSeconds(.5f);

            _rigidbody.velocity = Vector3.zero;;

            yield return new WaitForSeconds(3.5f);

            transform.DOShakePosition(.5f, Vector3.one.SetY(0f), 40);

            yield return new WaitForSeconds(.5f);

            _fallRecoveryRoutine = null;
            _state = State.Run;
            gameObject.SetLayerRecursively(_defaultLayer);

            // TODO: Walk animation
            _animator.Play("Walk");
            //_defaultMesh.SetActive(true);
            //_stunnedMesh.SetActive(false);
        }

        private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
        {
            return vector - vector.normalized * (friction * deltaTime);
        }

        private Player _player;
        public void GrabPlayer(Player player)
        {
            if (player.IsKnockedOut())
            {
                PlayerHasBeenKnockedOut();
            }
            else
            {
                _animator.Play("Shake");
            }
            
            _player = player;

            _state = State.Grabbed;
            _player.AddChaser(this);
            _player.PushPlayer(_rigidbody.velocity, 60);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            SoundManager.Instance.PlayGrabbedEffect();

            gameObject.SetLayerRecursively(_grabLayer);
            
        }


        public void PlayerHasBeenKnockedOut()
        {
            _animator.Play("FellDown");
        }

        void OnCollisionEnter(Collision collision)
        {
            if (_state == State.Fall || _state == State.Grabbed)
            {
                return;
            }

            var player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                GrabPlayer(player);
            }


            if (_fallRecoveryRoutine != null || _state != State.Charge)
            {
                return;
            }

            // TODO: Need to check if hitting wall at a vaugely 90 degree angle. Steal from wizcombat probably
            var chaser = collision.gameObject.GetComponent<Chaser>();
            if (chaser != null)
            {
                var direction = transform.position - chaser.transform.position;
                StopCoroutine(_chargeInDirectionRoutine);
                _chargeInDirectionRoutine = null;
                _fallRecoveryRoutine = FallRecovery(direction);
                StartCoroutine(_fallRecoveryRoutine);
            }

            // TODO: Need to check if both are charging in the general direction of each other
            var isWall = collision.gameObject.tag == "Barrier";
            if (isWall)
            {
                //var direction = transform.position - collision..transform.position;

                var directionViaNormal = collision.transform.forward;
                Debug.DrawRay(collision.contacts.First().point, directionViaNormal, Color.cyan, 10f);

                var direction = collision.transform.forward;
                StopCoroutine(_chargeInDirectionRoutine);
                _chargeInDirectionRoutine = null;
                _fallRecoveryRoutine = FallRecovery(direction);
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

                if (_player != null)
                {
                    _player.LoseChaser(this);
                    _player = null;
                }
            }
        }

        
    }
}
