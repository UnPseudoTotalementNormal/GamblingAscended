using System;
using GameEvents;
using Possess;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputContexts<GameEventInfo> _moveInputEvent;
    [SerializeField] private InputContexts<GameEventInfo> _jumpInputEvent;
    [SerializeField] private GameEvent<GameEventInfo> _mouseMoveInputEvent;
    [SerializeField] private InputContexts<GameEventInfo> _interactInputEvent;
    [SerializeField] private InputContexts<GameEventInfo> _placeInputEvent;

    [SerializeField] private GameObject _playerPrefab;

    private void Awake()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject newPlayer = Instantiate(_playerPrefab);
        newPlayer.Possess();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        var gameEventInfo = new GameEventVector2()
        {
            Value = value,
        };
        if (context.started) _moveInputEvent.EventStarted?.Invoke(gameEventInfo);
        if (context.performed) _moveInputEvent.EventPerformed?.Invoke(gameEventInfo);
        if (context.canceled) _moveInputEvent.EventCancel?.Invoke(gameEventInfo);
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        var gameEventInfo = new GameEventInfo();
        if (context.started) _jumpInputEvent.EventStarted?.Invoke(gameEventInfo);
        if (context.performed) _jumpInputEvent.EventPerformed?.Invoke(gameEventInfo);
        if (context.canceled) _jumpInputEvent.EventCancel?.Invoke(gameEventInfo);
    }
    
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        var gameEventInfo = new GameEventVector2()
        {
            Value = value,
        };
        _mouseMoveInputEvent?.Invoke(gameEventInfo);
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        var gameEventInfo = new GameEventInfo();
        if (context.started) _interactInputEvent.EventStarted?.Invoke(gameEventInfo);
        if (context.performed) _interactInputEvent.EventPerformed?.Invoke(gameEventInfo);
        if (context.canceled) _interactInputEvent.EventCancel?.Invoke(gameEventInfo);
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        var gameEventInfo = new GameEventInfo();
        if (context.started) _placeInputEvent.EventStarted?.Invoke(gameEventInfo);
        if (context.performed) _placeInputEvent.EventPerformed?.Invoke(gameEventInfo);
        if (context.canceled) _placeInputEvent.EventCancel?.Invoke(gameEventInfo);
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
