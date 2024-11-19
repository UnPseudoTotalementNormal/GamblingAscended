using System;
using Extensions;
using GameEvents;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinSlot : MonoBehaviour
    {
        private GL_CoinHolder _coinHolder;
        
        [SerializeField] private GameEvent<GameEventInfo> _tryInsertMoneyEvent;
        [SerializeField] private GameEvent<GameEventInfo> _moneyInsertedEvent;

        private void Awake()
        {
            _tryInsertMoneyEvent.AddListener(OnTryInsertMoney);
        }
        
        private void OnTryInsertMoney(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject) ||
                !gameEventGameObject.Value.TryGetComponentInParents(out GL_ICoinHolder coinHolder))
            {
                return;
            }
            
            if (coinHolder.MoneyInserted < 1)
            {
                return;
            }
            
            coinHolder.RemoveMoney(1);
            InsertCoin(1);
        }

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