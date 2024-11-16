using System;
using System.Collections.Generic;
using Breezorio.Ghosts;
using GamblingScripts.BasicComponents;
using UnityEngine;

namespace GamblingScripts
{
    public abstract class GL_GamblingMachine : MonoBehaviour, GL_IGamblingMachine, GL_IStateMachine
    {
        public int CurrentState { get; protected set; }
        public Dictionary<int, Action> StatesActions { get; protected set; }
        public Dictionary<int, Action> StatesCheckSwitch { get; protected set; }
        public Dictionary<int, Action> StatesSwitchAction { get; protected set; }
        [field:SerializeField] public float PlayMoneyCost { get; protected set; }
        public event Action PlayMachineEvent;

        void GL_IStateMachine.SetCurrentState(int state)
        {
            CurrentState = state;
        }
        public virtual float GetMoneyInserted()
        {
            if (TryGetComponent(out GL_CoinHolder _coinHolder))
            {
                return _coinHolder.MoneyInserted;
            }

            return 0;
        }

        public virtual bool TryPlay()
        {
            bool canPlay = GetMoneyInserted() >= PlayMoneyCost;
            if (canPlay) ((GL_IGamblingMachine)this).Play();
            return canPlay;
        }

        public virtual void Play()
        {
            PlayMachineEvent?.Invoke();
        }

        protected abstract void Awake();
        protected abstract void Start();
        protected abstract void Update();
    }
}