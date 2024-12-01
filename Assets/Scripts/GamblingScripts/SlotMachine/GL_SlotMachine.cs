using System;
using System.Collections.Generic;
using System.Linq;
using Breezorio.Ghosts;
using GamblingScripts.SlotMachine;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GL_SlotMachine : GL_BaseGamblingMachine
{
    [Header("Slot Machine Specific")] 
    [SerializeField] private List<GL_SlotMachineWheel> _slotWheels;
    
    [Header("Spinning State")]
    [SerializeField] private float _spinningDuration = 1f;
    [SerializeField] private float _finishSpinningRowDuration = 0.5f;

    [Header("GameEvents")] 
    [SerializeField] private GameEvent<GameEventInfo> _slotMachineTryPullLeverEvent;
    
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
        
        _slotMachineTryPullLeverEvent.AddListener(OnPullLever);
    }

    private void OnPullLever(GameEventInfo eventInfo)
    {
        if (gameObject.HasGameID(eventInfo.Ids))
        {
            if (!TryPlay())
            {
                return;
            }

            if (!eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
            {
                return;
            }

            LastPlayer = gameEventGameObject.Value;
        }
    }

    protected override void Start()
    {
        base.Start();
    }
    
    public override bool TryPlay()
    {
        bool canPlay = true;

        if (CurrentState != (int)SlotMachineState.None)
        {
            return canPlay = false;
        }
        
        canPlay = base.TryPlay();

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

    private void StartSpinning()
    {
        foreach (GL_SlotMachineWheel imagesRow in _slotWheels)
        {
            imagesRow.StartRolling();
        }
    }

    private void OnFinishSpinning()
    {
        (this as GL_IStateMachine).DoSwitchAction((int)SlotMachineState.Result);
    }
    
    private void OnFinishSpinningARow()
    {
        GameEventEnum.SM_OnRowStop.Invoke(new GameEventInfo { Ids = new [] { gameObject.GetGameID() }});
        _slotWheels.First(imagesRow => imagesRow.IsRolling).StopRolling();
        
        bool hasARowLeft = _slotWheels.Any(imagesRow => imagesRow.IsRolling);
        if (hasARowLeft)
        {
            Timer.Timer.NewTimer(_finishSpinningRowDuration, OnFinishSpinningARow);
            return;
        }

        OnFinishSpinning();
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
        StartSpinning();
        Timer.Timer.NewTimer(_spinningDuration, OnFinishSpinningARow);
    }

    private void SM_Result_SwitchAction()
    {
        CalculateResult();
        Timer.Timer.NewTimer(0.5f, () => (this as GL_IStateMachine).DoSwitchAction((int)SlotMachineState.None));
    }

    #endregion
    
    private void CalculateResult()
    {
        Dictionary<GL_SlotMachineImage, int> result = new();
        foreach (GL_SlotMachineImage image in _slotWheels.Select(slotWheel => slotWheel.GetResultImage()))
        {
            if (result.TryGetValue(image, out int count))
            {
                result[image] = count + 1;
            }
            else
            {
                result[image] = 1;
            }
        }

        float moneyAmount = result.Keys.Sum(slotImage => slotImage.ValuesOnAmount[result[slotImage]]);

        foreach (GL_SlotMachineImage slotImage in result.Keys)
        {
            var eventInfo = new GameEventInfo
            {
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject
            };
            slotImage.GameEventOnAmount.TryGetValue(result[slotImage], out GameEvent<GameEventInfo> gameEvent);
            gameEvent?.Invoke(eventInfo);
        }
        
        if (moneyAmount == 0)
        {
            return;
        }
        
        CoinHolder.AddMoney(moneyAmount);
    }
}
