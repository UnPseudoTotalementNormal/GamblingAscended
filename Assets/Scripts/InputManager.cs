using System;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputContexts<Vector2> _moveInputEvent;
    [SerializeField] private InputContexts _jumpInputEvent;
    [SerializeField] private GameEvent<Vector2> _mouseMoveInputEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        if (context.started) _moveInputEvent.EventStarted.Invoke(value);
        if (context.performed) _moveInputEvent.EventPerformed.Invoke(value);
        if (context.canceled) _moveInputEvent.EventCancel.Invoke(value);
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) _jumpInputEvent.EventStarted.Invoke();
        if (context.performed) _jumpInputEvent.EventPerformed.Invoke();
        if (context.canceled) _jumpInputEvent.EventCancel.Invoke();
    }
    
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _mouseMoveInputEvent.Invoke(value);
    }

    [Serializable]
    public struct InputContexts
    {
        public GameEvent EventStarted;
        public GameEvent EventPerformed;
        public GameEvent EventCancel;
    }
    
    [Serializable]
    public struct InputContexts<T>
    {
        public GameEvent<T> EventStarted;
        public GameEvent<T> EventPerformed;
        public GameEvent<T> EventCancel;
    }
}
