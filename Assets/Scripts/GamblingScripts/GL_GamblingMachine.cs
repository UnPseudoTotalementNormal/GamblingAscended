using System;
using System.Collections.Generic;
using Breezorio.Ghosts;
using UnityEngine;

namespace GamblingScripts
{
    public abstract class GL_GamblingMachine : MonoBehaviour, GL_IGamblingMachine, GL_IStateMachine
    {
        public int CurrentState { get; protected set; }
        public Dictionary<int, Action> StatesActions { get; protected set; }
        public Dictionary<int, Action> StatesCheckSwitch { get; protected set; }
        public Dictionary<int, Action> StatesSwitchAction { get; protected set; }

        void GL_IStateMachine.SetCurrentState(int state)
        {
            CurrentState = state;
        }

        protected abstract void Awake();
        protected abstract void Start();
        protected abstract void Update();
    }
}