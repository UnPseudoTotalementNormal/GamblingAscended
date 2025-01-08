using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameEvents;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Character.Enemy
{
    public class GL_PathFollower : MonoBehaviour
    {
        private GL_CharacterMovement _characterMovement;
        private Rigidbody _rigidbody;

        [SerializeField] private Transform _model;
        
        private int _currentWaypointIndex;
        public float CurrentWaypointDistance;
        public Vector3 CurrentWaypoint;
        public float CurrentDistance;

        public Dictionary<float, Vector3> _waypoints;

        [SerializeField] private GameEvent<GameEventInfo> _onPathTraced;
        
        private bool _isInit;
        
        public void Init(Dictionary<float, Vector3> pathTracerWaypoints)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _characterMovement = GetComponent<GL_CharacterMovement>();
            
            _waypoints = pathTracerWaypoints;
            CheckNextWaypoint();
            
            _onPathTraced?.AddListener(OnPathTraced);

            _isInit = true;
        }

        private void OnPathTraced(GameEventInfo eventInfo)
        {
            if (!eventInfo.TryTo(out GameEventPathTraced gameEventPathTraced))
            {
                return;
            }
            
            _waypoints = gameEventPathTraced.PathTracerWaypoints;
            
            // Get the closest waypoint as the target
            var closestWaypoint = _waypoints.OrderBy(wp => Vector3.Distance(transform.position, wp.Value)).First();
            _currentWaypointIndex = _waypoints.Keys.ToList().IndexOf(closestWaypoint.Key);
            CurrentWaypoint = closestWaypoint.Value;

            // Calculate the new distance based on the closest waypoint
            CurrentDistance = closestWaypoint.Key - Vector3.Distance(transform.position, CurrentWaypoint);
            CurrentWaypointDistance = closestWaypoint.Key;
        }

        private void Update()
        {
            if (!_isInit)
            {
                return;
            }
            
            CheckNextWaypoint();
            
            _model.forward = Vector3.Lerp(_model.forward, (CurrentWaypoint - transform.position).ToFlatVector3().normalized, Time.deltaTime * 5f);
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