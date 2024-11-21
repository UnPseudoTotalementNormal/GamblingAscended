using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Boids
{
    public class GL_BoidSystem : MonoBehaviour
    {
        [SerializeField] private Transform _boidPrefab;
        [FormerlySerializedAs("attractor")] [SerializeField] private Transform _attractor;
        [SerializeField] private int _count;

        [SerializeField] private BoidSettings _settings;
        
        private Boid[] _boids;
        private BoidRegions _regions = new();
        private float _regionSize => _settings.FarThreshold;
        
        struct Boid : IEquatable<Boid>
        {
            public Transform Transform;
            public Vector3 Velocity;
            public Vector3 Position;

            public Vector3 NextVelocity;

            private Vector3Int _currentRegion;

            public void ComputeNextVelocity(BoidSettings settings, BoidRegions allRegions)
            {
                Vector3 alignement = Vector3.zero;
                Vector3 avoidance = Vector3.zero;
                Vector3 cohesion = Vector3.zero;
                Vector3 attraction = Vector3.zero;

                int counter = 0;
                foreach (Boid testBoid in allRegions.GetBoidsNearTo(_currentRegion))
                {
                    if (Equals(testBoid, this))
                    {
                        continue;
                    }

                
                    //alignement
                    Vector3 direction = testBoid.Velocity;
                    float distance = Vector3.Distance(testBoid.Position, Position);
                    alignement += Vector3.ClampMagnitude(direction / Mathf.Max(distance, 0.01f), 1);
                
                    //avoidance
                    direction = Position - testBoid.Position;
                    distance /= settings.FarThreshold;
                    avoidance += direction.normalized * (1 - distance);
                
                    //cohesion
                    direction *= -1;
                    if (distance > settings.FarThreshold)
                    {
                        cohesion += Vector3.ClampMagnitude(direction.normalized * (distance - 1), 1);
                    }
                    
                    //max iteration
                    if (counter++ > settings.MaxIterations)
                    {
                        break;
                    }
                }

                NextVelocity = alignement * settings.Alignment +
                               avoidance * settings.Avoidance +
                               cohesion * settings.Cohesion +
                               ((settings.AttractorPosition - Position).normalized * settings.Attraction);
                NextVelocity.Normalize();
            }

            public void ApplyNextVelocity(BoidSettings settings, BoidRegions allRegions)
            {
                Velocity = Vector3.Slerp(Velocity, NextVelocity, settings.TurnRate);
                Position = Transform.position += Velocity * (settings.Speed * Time.deltaTime);
                Transform.forward = Velocity;

                var newRegion = new Vector3Int
                {
                    x = Mathf.FloorToInt(Transform.position.x / settings.FarThreshold),
                    y = Mathf.FloorToInt(Transform.position.y / settings.FarThreshold),
                    z = Mathf.FloorToInt(Transform.position.z / settings.FarThreshold)
                };

                if (newRegion == _currentRegion) return;
                
                allRegions[_currentRegion].Remove(this);

                _currentRegion = newRegion;

                if (!allRegions.ContainsKey(newRegion))
                {
                    allRegions[newRegion] = new List<Boid>();
                }
                    
                allRegions[_currentRegion].Add(this);
            }

            public bool Equals(Boid other)
            {
                return Equals(Transform, other.Transform) && Velocity.Equals(other.Velocity) && NextVelocity.Equals(other.NextVelocity) && _currentRegion.Equals(other._currentRegion);
            }

            public override bool Equals(object obj)
            {
                return obj is Boid other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Transform, Velocity, NextVelocity, _currentRegion);
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
            public int MaxIterations;

            [HideInInspector] public Vector3 AttractorPosition;
        }

        class BoidRegions : Dictionary<Vector3Int, List<Boid>>
        {
            public IEnumerable<Boid> GetBoidsNearTo(Vector3Int region)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            Vector3Int testedRegionPos = region + new Vector3Int(x, y, z);

                            if (!TryGetValue(testedRegionPos, out List<Boid> testingRegion))
                            {
                                continue;
                            }

                            foreach (Boid boid in testingRegion)
                            {
                                yield return boid;
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            _settings.AttractorPosition = _attractor.position;
            ComputeNextVelocity();
            ApplyNextVelocity();
        }

        private void ComputeNextVelocity()
        {
            Parallel.For(0, _boids.Length, i =>
            {
                _boids[i].ComputeNextVelocity(_settings, _regions);
            });
        }
        
        private void ApplyNextVelocity()
        {
            for (int i = 0; i < _boids.Length; i++)
            {
                _boids[i].ApplyNextVelocity(_settings, _regions);
            }
        }

        private void Start()
        {
            _boids = new Boid[_count];
            _regions.Add(Vector3Int.zero, new List<Boid>());
            
            for (int i = 0; i < _count; i++)
            {
                _boids[i] = new Boid 
                { 
                    Transform = Instantiate(_boidPrefab, transform), 
                    Velocity = Random.onUnitSphere
                };
                _regions[Vector3Int.zero].Add(_boids[i]);

                MeshRenderer renderer = _boids[i].Transform.GetComponentInChildren<MeshRenderer>();
                renderer.material = new Material(renderer.material);
                renderer.material.color = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
            }
        }
    }
}

