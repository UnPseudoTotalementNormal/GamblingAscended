using System;
using GameEvents;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_CameraController : MonoBehaviour
{
    private Transform _transform;
    
    [SerializeField] private GameEvent<Vector2> _mouseInputEvent;
    [SerializeField] private float _sensitivity = 1;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _mouseInputEvent.AddListener(OnMouseMoved);
    }

    private void OnMouseMoved(int[] arg1, Vector2 value)
    {
        value *= _sensitivity;
        _transform.Rotate(-value.y, value.x, 0);
        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, 0);
    }
}
