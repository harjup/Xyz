using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Xyz.Scripts
{
    public class Chaser : MonoBehaviour
    {
        private GameObject _player;
        private Rigidbody _rigidbody;

        public float Acceleration = 640;
        public float Friction = 40f;
        public float MaxSpeed = 10f;


        public void Start()
        {
            _player = FindObjectOfType<Move>().gameObject;
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            var inputVector = (_player.transform.position - transform.position).normalized;
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

        private Vector3 ApplyFriction(Vector3 vector, float friction, float deltaTime)
        {
            return vector - vector.normalized * (friction * deltaTime);
        }
    }
}
