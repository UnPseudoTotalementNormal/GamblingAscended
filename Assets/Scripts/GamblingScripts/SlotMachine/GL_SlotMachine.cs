using System;
using System.Collections.Generic;
using Breezorio.Ghosts;
using GamblingScripts.SlotMachine;
using UnityEngine;
using UnityEngine.UI;


public class GL_SlotMachine : GL_BaseGamblingMachine
{
    [Header("Slot Machine Specific")] 
    [SerializeField] private List<GL_SlotMachineImage> _resultImages;
    [SerializeField] private List<GL_SlotMachineImageRow> _imagesRows;
    
    [Header("Spinning State")]
    [SerializeField] private float _spinningDuration = 1f;
    
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
            {(int) SlotMachineState.Spinning, SM_Spinning_SwitchAction},
            {(int) SlotMachineState.Result, SM_Result_SwitchAction},
        };
    }
    
    protected override void Start()
    {
        base.Start();
        Play();
    }
    
    public override bool TryPlay()
    {
        bool canPlay = base.TryPlay();
        if (!canPlay) return false;

        if (CurrentState != (int)SlotMachineState.None) canPlay = false;

        return canPlay;
    }

    public override void Play()
    {
        base.Play();
        (this as GL_IStateMachine).DoSwitchAction((int)SlotMachineState.Spinning);
    }
    
    private void ResetSlotMachine()
    {
        
    }
    
    private void OnFinishSpinning()
    {
        foreach (GL_SlotMachineImageRow imagesRow in _imagesRows)
        {
            Transform testImage = imagesRow.transform.GetChild(0);
            GL_SlotMachineImage resultImage = _resultImages.PickRandom();
            testImage.GetComponent<Image>().sprite = resultImage.ObjectSprite;
        }
        (this as GL_IStateMachine).DoSwitchAction((int)SlotMachineState.Result);
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

    private void SM_Spinning_SwitchAction()
    {
        Timer.Timer.NewTimer(_spinningDuration, OnFinishSpinning);
    }

    private void SM_Result_SwitchAction()
    {
        Timer.Timer.NewTimer(2f, () => (this as GL_IStateMachine).DoSwitchAction((int)SlotMachineState.None));
    }

    #endregion
}
