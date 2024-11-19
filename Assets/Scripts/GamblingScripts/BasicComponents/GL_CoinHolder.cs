using System;
using System.Linq;
using GameEvents;
using GameEvents.GameEventDefs;
using UnityEngine;
using GameEventFloat = GameEvents.GameEventFloat;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinHolder : MonoBehaviour
    {
        public float MoneyInserted { get; private set; } = 0;

        [SerializeField] private GameEvent<GameEventInfo> _moneyInsertedEvent;
        [SerializeField] private GameEvent<GameEventInfo> _playMachineEvent;
        
        private void Start()
        {
            _moneyInsertedEvent.AddListener(AddMoney);
            _playMachineEvent.AddListener(RemoveMoney);
        }

        private void AddMoney(int[] ids, GameEventInfo gameEventInfo)
        {
            if (!gameObject.HasGameID(ids) || !gameEventInfo.TryTo(out GameEventFloat gameEventFloat))
            {
                return;
            }
            
            MoneyInserted += gameEventFloat.Value;
        }

        private void RemoveMoney(int[] ids, GameEventInfo gameEventInfo)
        {
            if (!gameObject.HasGameID(ids) || !gameEventInfo.TryTo(out GameEventFloat gameEventFloat))
            {
                return;
            }
            MoneyInserted -= gameEventFloat.Value;
        }
    }
}