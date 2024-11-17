using System;
using GameEvents;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinSlot : MonoBehaviour
    {
        private GL_CoinHolder _coinHolder;
        
        public event Action<float> CoinInsertedEvent;
        
        [SerializeField] private GameEvent<float> _moneyInsertedEvent;

        public void InsertCoin(float value)
        {
            CoinInsertedEvent?.Invoke(value);
            _moneyInsertedEvent.Invoke(value, gameObject.GetGameID());
        }
    }
}