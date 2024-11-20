using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boids
{
    public class GL_BoidSystem : MonoBehaviour
    {
        [SerializeField] private Transform _boidPrefab;
        [SerializeField] private int _count;

        [SerializeField] private BoidSettings _settings;
        
        private Boid[] _boids;
        
        struct Boid
        {
            public Transform Transform;
            public Vector3 Velocity;
            public Vector3 Position => Transform.position;

            public Vector3 NextVelocity;

            public void ComputeNextVelocity(Boid[] boids, BoidSettings settings)
            {
                Vector3 alignement = Vector3.zero;
                Vector3 avoidance = Vector3.zero;
                Vector3 cohesion = Vector3.zero;
                
                for (int i = 0; i < boids.Length; i++)
                {
                    if (Equals(boids[i], this))
                    {
                        continue;
                    }
                    
                    //alignement
                    alignement += boids[i].Velocity;
                    
                    //avoidance
                    Vector3 direction = Position - boids[i].Position;
                    float distance = direction.magnitude / settings.FarThreshold;
                    
                    avoidance += direction.normalized * (1 - distance);
                    
                    //cohesion
                    direction *= -1;
                    if (distance > settings.FarThreshold)
                    {
                        cohesion += Vector3.ClampMagnitude(direction.normalized * (distance - 1), 1);
                    }
                }

                NextVelocity = alignement * settings.Alignment +
                               avoidance * settings.Avoidance +
                               cohesion * settings.Cohesion;
                NextVelocity.Normalize();
            }

            public void ApplyNextVelocity(Boid[] boids, BoidSettings settings)
            {
                Velocity = Vector3.Slerp(Velocity, NextVelocity, settings.TurnRate);
                Transform.position += Velocity * (settings.Speed * Time.deltaTime);
            }
        }

        [Serializable]
        private class BoidSettings
        {
            public float Alignment;
            public float Avoidance;
            public float Cohesion;
            public float Attraction;
            public float FarThreshold;
            public float Speed;
            public float TurnRate;
        }

        private void Update()
        {
            ComputeNextVelocity();
            ApplyNextVelocity();
        }

        private void ComputeNextVelocity()
        {
            for (int i = 0; i < _boids.Length; i++)
            {
                _boids[i].ComputeNextVelocity(_boids, _settings);
            }
        }
        
        private void ApplyNextVelocity()
        {
            for (int i = 0; i < _boids.Length; i++)
            {
                _boids[i].ApplyNextVelocity(_boids, _settings);
            }
        }

        private void Start()
        {
            _boids = new Boid[_count];
            
            for (int i = 0; i < _count; i++)
            {
                _boids[i] = new Boid 
                { 
                    Transform = Instantiate(_boidPrefab, transform), 
                    Velocity = Random.onUnitSphere
                };
            }
        }
    }
}

