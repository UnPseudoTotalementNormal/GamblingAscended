using System;
using System.Linq;
using GameEvents;
using GameEvents.GameEventDefs;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinHolder : MonoBehaviour
    {
        public float MoneyInserted { get; private set; } = 0;

        [SerializeField] private GameEvent<float> _moneyInsertedEvent;
        [SerializeField] private GameEvent<float> _playMachineEvent;
        
        private void Start()
        {
            _moneyInsertedEvent.AddListener(AddMoney);
            _playMachineEvent.AddListener(RemoveMoney);
        }

        private void AddMoney(int[] ids, float value)
        {
            if (!gameObject.HasGameID(ids))
            {
                return;
            }
            MoneyInserted += value;
        }

        private void RemoveMoney(int[] ids, float value)
        {
            if (!gameObject.HasGameID(ids))
            {
                return;
            }
            MoneyInserted -= value;
        }
    }
}