using System;
using System.Collections.Generic;

namespace Breezorio.Ghosts
{
    public interface GL_IStateMachine
    {  
        int CurrentState { get; }
        Dictionary<int, Action> StatesActions { get;  }
        Dictionary<int, Action> StatesCheckSwitch { get; }
        Dictionary<int, Action> StatesSwitchAction { get;}

        protected internal void SetCurrentState(int state);
        
        /// <summary>
        /// Executes an action based on the current state.
        /// </summary>
        public void DoAction()
        {
            if (StatesActions.ContainsKey(CurrentState))
            {
                StatesActions[CurrentState]();
            }
        }

        /// <summary>
        /// Switches to a new state based on the current state.
        /// </summary>
        public void DoCheckSwitch()
        {
            if (StatesCheckSwitch.ContainsKey(CurrentState))
            {
                StatesCheckSwitch[CurrentState]();
            }
        }
        
        /// <summary>
        /// Switches to a new state and execute an action based on the new state
        /// </summary>
        public void DoSwitchAction(int newState)
        {
            SetCurrentState(newState);
            if (StatesSwitchAction.ContainsKey(CurrentState))
            {
                StatesSwitchAction[newState]();
            }
        }

        public void DoSwitchAction(Enum newState)
        {
            DoSwitchAction(Convert.ToInt32(newState));
        }
    }
}