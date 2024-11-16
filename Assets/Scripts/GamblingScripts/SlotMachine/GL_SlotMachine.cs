using System;
using System.Collections.Generic;
using Breezorio.Ghosts;
using UnityEngine;


public class GL_SlotMachine : GL_BaseGamblingMachine
{
    [Header("Slot Machine Specific")] 
    [SerializeField] private List<GL_SlotMachineImage> _resultImages;
    [SerializeField] private Transform _leftImagesRow;
    [SerializeField] private Transform _middleImagesRow;
    [SerializeField] private Transform _rightImagesRow;
    
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
            {(int) SlotMachineState.Result, () => {}}
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

    #region SM_Actions

    private void SM_Spinning_Action()
    {
        
    }
    
    private void SM_Result_Action()
    {
        
    }
    
    #endregion

    #region SM_CheckSwitch

    private void SM_Result_CheckSwitch()
    {
        
    }

    #endregion
}
