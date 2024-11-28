using System;
using System.Linq;
using GameEvents;
using GameEvents.GameEventDefs;
using UnityEngine;
using GameEventFloat = GameEvents.GameEventFloat;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinHolder : MonoBehaviour, GL_ICoinHolder
    {
        public float MoneyInserted { get; private set; } = 0;
        [field:SerializeField] public float BaseMoney { get; private set; }

        [SerializeField] private GameEvent<GameEventInfo> _moneyInsertedEvent;
        [SerializeField] private GameEvent<GameEventInfo> _playMachineEvent;
        [SerializeField] private GameEvent<GameEventInfo> _cashoutMoneyEvent;
        
        private void Start()
        {
            _moneyInsertedEvent?.AddListener(OnInsertedMoney);
            _playMachineEvent?.AddListener(OnPlayMachine);
            _cashoutMoneyEvent?.AddListener(OnCashoutMoney);
            AddMoney(BaseMoney);
        }

        private void OnCashoutMoney(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
            {
                return;
            }
            
            gameEventGameObject.Value.GetComponentInParent<GL_ICoinHolder>().AddMoney(MoneyInserted);
            MoneyInserted = 0;
        }

        private void OnPlayMachine(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventFloat gameEventFloat))
            {
                return;
            }
            
            RemoveMoney(gameEventFloat.Value);
        }

        private void OnInsertedMoney(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventFloat gameEventFloat))
            {
                return;
            }

            AddMoney(gameEventFloat.Value);
        }

        public void AddMoney(float value)
        {
            MoneyInserted += value;
            MoneyInserted = Math.Max(0, MoneyInserted);
        }

        public void RemoveMoney(float value)
        {
            MoneyInserted -= value;
            MoneyInserted = Math.Max(0, MoneyInserted);
        }
    }
}