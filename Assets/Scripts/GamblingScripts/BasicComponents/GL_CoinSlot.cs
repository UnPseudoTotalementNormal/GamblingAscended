using System;
using GameEvents;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinSlot : MonoBehaviour
    {
        private GL_CoinHolder _coinHolder;
        
        [SerializeField] private GameEvent<GameEventInfo> _tryInsertMoneyEvent;
        [SerializeField] private GameEvent<GameEventInfo> _moneyInsertedEvent;

        public void InsertCoin(float value)
        {
            var gameEventFloat = new GameEventFloat()
            {
                Value = value,
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            _moneyInsertedEvent.Invoke(gameEventFloat);
        }
    }
}