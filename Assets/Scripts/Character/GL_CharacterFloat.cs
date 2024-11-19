using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class GL_CharacterFloat : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    
    [SerializeField] private float _floatHeight = 0.5f;
    [SerializeField] private float _rayLength = 1f;
    [FormerlySerializedAs("_springStrength")] [SerializeField] private float _floatSpringStrength = 20;
    [FormerlySerializedAs("_dampingStrength")] [SerializeField] private float _floatDampingStrength = 5;
    [SerializeField] private float _maxSpringForce = 50;
    
    [SerializeField] private float _uprightSpringStrength = 20;
    [SerializeField] private float _uprightDampingStrength = 5;
    
    

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        FloatCharacter();
        UpdateUprightForce();
    }

    private void UpdateUprightForce()
    {
        Vector3 wantedRot = Vector3.zero;
        wantedRot.y += _transform.eulerAngles.y;
        Quaternion uprightQuat = Quaternion.Euler(wantedRot);
        
        Quaternion characterCurrent = _transform.rotation;
        Quaternion toGoal = ShortestRotation(uprightQuat, characterCurrent);

        Vector3 rotAxis;
        float rotDegrees;
        
        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;
        
        _rigidbody.AddTorque((rotAxis * (rotRadians * _uprightSpringStrength)) - (_rigidbody.angularVelocity * _uprightDampingStrength));
    }
    
    public static Quaternion ShortestRotation(Quaternion from, Quaternion to)
    {
        return Quaternion.Inverse(from) * to;
    }
    
    private void FloatCharacter()
    {
        Ray ray = new Ray(_transform.position, Vector3.down);
        RaycastHit hitInfo;
        
        if (!Physics.Raycast(ray, out hitInfo, _rayLength))
        {
            return;
        }

        float currentHeight = _transform.position.y;
        float targetHeight = hitInfo.point.y + _floatHeight;
        float heightDifference = targetHeight - currentHeight;

        float springForce = heightDifference * _floatSpringStrength;
        float dampingForce = -_rigidbody.linearVelocity.y * _floatDampingStrength;
            
        float gravityCompensation = _rigidbody.mass * Physics.gravity.magnitude;
            
        float totalForce = springForce + dampingForce + gravityCompensation;
        totalForce = Mathf.Clamp(totalForce, -_maxSpringForce, _maxSpringForce);
            
        _rigidbody.AddForce(Vector3.up * totalForce);
    }
}
