using System;
using UnityEngine;

namespace GamblingScripts.BasicComponents
{
    public class GL_CoinSlot : MonoBehaviour
    {
        private GL_CoinHolder _coinHolder;
        
        public event Action<float> CoinInsertedEvent;

        public void InsertCoin(float value)
        {
            CoinInsertedEvent?.Invoke(value);
        }
    }
}