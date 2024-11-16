using System;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinHolder : MonoBehaviour
    {
        public float MoneyInserted { get; private set; } = 0;

        private void Start()
        {
            if (TryGetComponent(out GL_GamblingMachine _gamblingMachine))
            {
                _gamblingMachine.PlayMachineEvent += () => RemoveMoney(_gamblingMachine.PlayMoneyCost);
            }

            if (TryGetComponent(out GL_CoinSlot _coinSlot))
            {
                _coinSlot.CoinInsertedEvent += AddMoney;
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