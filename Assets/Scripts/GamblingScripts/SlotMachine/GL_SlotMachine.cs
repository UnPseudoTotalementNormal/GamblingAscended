using System;
using System.Collections.Generic;
using Breezorio.Ghosts;
using UnityEngine;


public class GL_SlotMachine : GL_BaseGamblingMachine
{
    [Header("Slot Machine Specific")] 
    [SerializeField] private List<GL_SlotMachineImage> _resultImages;
    [SerializeField] private List<Transform> _imagesRows;
    
    public enum SlotMachineState 
    {
        None,
        Spinning,
        Result,
    }
    
    protected override void Awake()
    {
        base.Awake();
        StatesActions = new Dictionary<int, Action>
        {
            {(int) SlotMachineState.None, () => {}},
            {(int) SlotMachineState.Spinning, SM_Spinning_Action},
            {(int) SlotMachineState.Result, SM_Result_Action},
        };
        
        StatesCheckSwitch = new Dictionary<int, Action>
        {
            {(int) SlotMachineState.None, () => {}},
            {(int) SlotMachineState.Spinning, () => {}},
            {(int) SlotMachineState.Result, SM_Result_CheckSwitch},
        };

        StatesSwitchAction = new Dictionary<int, Action>
        {
            {(int) SlotMachineState.None, SM_None_SwitchAction},
            {(int) SlotMachineState.Spinning, () => {}},
            {(int) SlotMachineState.Result, () => {}},
        };
    }
    
    protected override void Start()
    {
        base.Start();
        Play();
    }

    public override void Play()
    {
        base.Play();
        
    }
    
    private void ResetSlotMachine()
    {
        
    }

    #region SM_Actions

    private void SM_Spinning_Action()
    {
        
    }
    
    private void SM_Result_Action()
    {
        
    }
    
    #endregion

    #region SM_CheckSwitchs

    private void SM_Result_CheckSwitch()
    {
        
    }

    #endregion

    #region SM_SwitchActions

    private void SM_None_SwitchAction()
    {
        ResetSlotMachine();
    }

    private void SM_Result_SwitchAction()
    {
        Timer.Timer.NewTimer(2f, () => (this as GL_IStateMachine).DoSwitchAction((int)SlotMachineState.None));
    }

    #endregion
}
