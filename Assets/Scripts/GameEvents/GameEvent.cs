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
        
        public Action<T> Action { get; private set; }
        private List<Action<T>> actions = new();
        
        public void AddListener(Action<T> action) => actions.Add(action);
        
        public void RemoveListener(Action<T> action) => actions.Remove(action);
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Invoke(T value)
        {
            actions.RemoveAll(a => a == null);
            foreach (Action<T> action in actions.ToList())
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
        
        public void ClearListeners() => Action = null;
    }
}