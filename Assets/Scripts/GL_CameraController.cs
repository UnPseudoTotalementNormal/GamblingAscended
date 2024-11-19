using System;
using GameEvents;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_CameraController : MonoBehaviour
{
    private Transform _transform;
    
    [SerializeField] private GameEvent<GameEventInfo> _mouseInputEvent;
    [SerializeField] private float _sensitivity = 1;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _mouseInputEvent.AddListener(OnMouseMoved);
    }

    private void OnMouseMoved(GameEventInfo eventInfo)
    {
        if (!eventInfo.TryTo(out GameEventVector2 gameEventVector2))
        {
            return;
        }

        Vector2 moveInput = gameEventVector2.Value;
        moveInput *= _sensitivity;
        _transform.Rotate(-moveInput.y, moveInput.x, 0);
        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, 0);
    }
}
