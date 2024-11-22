using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Character.Enemy
{
    public class GL_PathFollower : MonoBehaviour
    {
        private GL_CharacterMovement _characterMovement;
        private Rigidbody _rigidbody;
        
        private int _currentWaypointIndex;
        public float CurrentWaypointDistance;
        public Vector3 CurrentWaypoint;
        public float CurrentDistance;

        public Dictionary<float, Vector3> _waypoints;

        private bool _isInit;
        
        public void Init(Dictionary<float, Vector3> pathTracerWaypoints)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _characterMovement = GetComponent<GL_CharacterMovement>();
            
            _waypoints = pathTracerWaypoints;
            CheckNextWaypoint();

            _isInit = true;
        }

        private void Update()
        {
            if (!_isInit)
            {
                return;
            }
            
            CheckNextWaypoint();
        }

        private void FixedUpdate()
        {
            if (!_isInit)
            {
                return;
            }

            Vector3 direction = CurrentWaypoint - transform.position;
            Vector2 flatDirection = new Vector2(direction.x, direction.z).normalized;
            _characterMovement.SetDirection(flatDirection);
            CurrentDistance += _rigidbody.linearVelocity.magnitude * Time.fixedDeltaTime;
        }

        public void CheckNextWaypoint()
        {
            if (CurrentDistance < CurrentWaypointDistance)
            {
                return;
            }
            
            List<float> keys = _waypoints.Keys.ToList();
            _currentWaypointIndex++;
            float value = keys[_currentWaypointIndex];

            CurrentWaypoint = _waypoints[value];
            CurrentWaypointDistance = value;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(CurrentWaypoint, Vector3.one * 1f);
        }
    }
}