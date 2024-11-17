using System;
using GameEvents.GameEventDefs;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinHolder : MonoBehaviour
    {
        public float MoneyInserted { get; private set; } = 0;

        private GameEvent<float> _moneyInsertedEvent;
        
        private void Start()
        {
            if (TryGetComponent(out GL_GamblingMachine _gamblingMachine))
            {
                _gamblingMachine.PlayMachineEvent += () => RemoveMoney(_gamblingMachine.PlayMoneyCost);
            }
        }

        private void AddMoney(float value)
        {
            MoneyInserted += value;
        }

        private void RemoveMoney(float value)
        {
            MoneyInserted -= value;
        }
    }
}