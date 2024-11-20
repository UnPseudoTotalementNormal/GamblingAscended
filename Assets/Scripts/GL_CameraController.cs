using System;
using GameEvents;
using Possess;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_CameraController : MonoBehaviour, GL_IPossessable
{
    private Transform _transform;
    
    [SerializeField] private GameEvent<GameEventInfo> _mouseInputEvent;
    [SerializeField] private float _sensitivity = 1;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
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

    void GL_IPossessable.OnPossess()
    {
        _mouseInputEvent.AddListener(OnMouseMoved);
    }

    void GL_IPossessable.OnUnpossess()
    {
        _mouseInputEvent.RemoveListener(OnMouseMoved);
    }
}
