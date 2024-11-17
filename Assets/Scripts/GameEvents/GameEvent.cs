using System;
using GameEvents;
using UnityEngine;

namespace GameEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/GameEvent", order = 0)]
    public class GameEvent : ScriptableObject, IGameEvent
    {
        public Action<int[]> Action { get; private set; }
        
        public void AddListener(Action<int[]> action) => Action += action;
        
        public void RemoveListener(Action<int[]> action) => Action -= action;
        
        public void Invoke(params int[] id) => Action?.Invoke(id);
    }
    
    public class GameEvent<T> : ScriptableObject, IGameEvent
    {
        [Header("Useless, for dev info only")]
        [SerializeField] private string param1Description;
        
        public Action<int[], T> Action { get; private set; }
        
        public void AddListener(Action<int[], T> action) => Action += action;
        
        public void RemoveListener(Action<int[], T> action) => Action -= action;
        
        public void Invoke(T value, params int[] id) => Action?.Invoke(id, value);
    }
    
    public class GameEvent<T1, T2> : ScriptableObject, IGameEvent
    {
        [Header("Useless, for dev info only")]
        [SerializeField] private string param1Description;
        [SerializeField] private string param2Description;
        
        public Action<int[], T1, T2> Action { get; private set; }
        
        public void AddListener(Action<int[], T1, T2> action) => Action += action;
        
        public void RemoveListener(Action<int[], T1, T2> action) => Action -= action;
        
        public void Invoke(T1 value1, T2 value2, params int[] id) => Action?.Invoke(id, value1, value2);
    }
}