using System;
using GameEvents;
using Possess;
using UnityEngine;

namespace Character
{
    public class GL_CharacterMovement : MonoBehaviour, GL_IPossessable
    {
        private Transform _transform;
        private Rigidbody _rigidbody;
        
        [SerializeField] private GameEvent<GameEventInfo> _moveInputEvent;

        [Header("Locomotion")]
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _maxAccelForce;

        [Header("Friction")] 
        [SerializeField] private Vector3 _frictionForce;
        
        private Vector2 _direction;
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnMoveInput(GameEventInfo eventInfo)
        {
            if (!eventInfo.TryTo(out GameEventVector2 gameEventVector2))
            {
                return;
            }

            SetDirection(gameEventVector2.Value);
        }
        
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            ApplyFriction();
            MoveCharacter(_direction);
        }

        private void ApplyFriction()
        {
            Vector3 frictionForce = Vector3.zero;
            frictionForce.Set(
                _frictionForce.x * -_rigidbody.linearVelocity.x, 
                _frictionForce.y * -_rigidbody.linearVelocity.y, 
                _frictionForce.z * -_rigidbody.linearVelocity.z);
            _rigidbody.AddForce(frictionForce, ForceMode.Acceleration);
        }

        private void MoveCharacter(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                return;
            }
    
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
            Vector3 currentVelocity = _rigidbody.linearVelocity;
            if (moveDirection.magnitude > 1)
            {
                moveDirection.Normalize();
            }
            moveDirection = _transform.TransformDirection(moveDirection);
            
            
            Vector3 acceleration = moveDirection * _acceleration;
            if (acceleration.magnitude > _maxAccelForce)
            {
                acceleration = acceleration.normalized * _maxAccelForce;
            }
            
            float accelDot = Vector3.Dot(moveDirection, currentVelocity.normalized);
            float maxAccel = Mathf.Min((_maxSpeed - (currentVelocity.magnitude * accelDot)) / Time.fixedDeltaTime, _maxAccelForce);
            maxAccel = Mathf.Max(maxAccel, 0);
            acceleration = Vector3.ClampMagnitude(acceleration, maxAccel);
            
            _rigidbody.AddForce(acceleration, ForceMode.Acceleration);
        }

        void GL_IPossessable.OnPossess()
        {
            _moveInputEvent.AddListener(OnMoveInput);
        }

        void GL_IPossessable.OnUnpossess()
        {
            _moveInputEvent.RemoveListener(OnMoveInput);
        }
    }
}