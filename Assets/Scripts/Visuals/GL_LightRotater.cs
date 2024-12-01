using System;
using UnityEngine;

public class GL_LightRotater : MonoBehaviour
{
    private Transform _transform;
    
    [SerializeField] private float _rotationSpeed = 1f;

    [SerializeField] private Vector3 _pointARotation;
    [SerializeField] private Vector3 _pointBRotation;

    private void LateUpdate()
    {
        if (!_transform)
        {
            _transform = GetComponent<Transform>();
        }
        
        _transform.rotation = Quaternion.Lerp(
            Quaternion.Euler(_pointARotation), 
            Quaternion.Euler(_pointBRotation), 
            (Mathf.Sin(Time.time * _rotationSpeed) + 1f) / 2f);
    }
}
