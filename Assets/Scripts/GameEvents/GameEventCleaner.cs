using System;
using GameEvents;
using UnityEngine;

public class GameEventCleaner : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;

    private void OnDestroy()
    {
        _gameEvent.ClearListeners();
    }
}
