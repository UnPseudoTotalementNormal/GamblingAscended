using System;
using System.Collections.Generic;
using System.Linq;
using GameEvents;
using UnityEngine;

namespace GameEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject, IGameEvent
    {
        public Action Action { get; private set; }
        
        public void AddListener(Action action) => Action += action;
        
        public void RemoveListener(Action  action) => Action -= action;
        
        public void Invoke() => Action?.Invoke();
        
        public void ClearListeners() => Action = null;
    }
    
    public class GameEvent<T> : ScriptableObject, IGameEvent
    {
        [Header("Useless, for dev info only")]
        [SerializeField] private string param1Description;
        
        private List<Action<T>> _actionList = new();
        
        public void AddListener(Action<T> action) => _actionList.Add(action);
        
        public void RemoveListener(Action<T> action) => _actionList.Remove(action);
        
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Invoke(T value)
        {
            _actionList.RemoveAll(a => a == null);
            foreach (Action<T> action in _actionList.ToList())
            {
                try
                {
                    action(value);
                }
                catch (Exception error)
                {
                    if (error is not MissingReferenceException)
                    {
                        Debug.LogException(error);
                    }
                    RemoveListener(action);
                }
            }
        }
        
        public void ClearListeners() => _actionList = new();
    }
}