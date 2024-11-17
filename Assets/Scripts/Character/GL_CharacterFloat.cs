using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class GL_CharacterFloat : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    
    [SerializeField] private float _floatHeight = 0.5f;
    [SerializeField] private float _rayLength = 1f;
    [SerializeField] private float _springStrength;
    [SerializeField] private float _dampingStrength;
    [SerializeField] private float _maxSpringForce;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(_transform.position, Vector3.down);
        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, _rayLength))
        {
            float currentHeight = _transform.position.y;
            float targetHeight = hit.point.y + _floatHeight;
            float heightDifference = targetHeight - currentHeight;

            float springForce = heightDifference * _springStrength;
            float dampingForce = -_rigidbody.linearVelocity.y * _dampingStrength;
            
            float gravityCompensation = _rigidbody.mass * Physics.gravity.magnitude;
            
            float totalForce = springForce + dampingForce + gravityCompensation;
            totalForce = Mathf.Clamp(totalForce, -_maxSpringForce, _maxSpringForce);
            
            _rigidbody.AddForce(Vector3.up * totalForce);
        }
    }
}
