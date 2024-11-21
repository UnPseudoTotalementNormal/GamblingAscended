using System;
using Extensions;
using GameEvents;
using UnityEngine;

[RequireComponent(typeof(GL_BaseGamblingMachine))]
public class GL_GamblingMachineReactEvent : MonoBehaviour
{
    private GL_BaseGamblingMachine _gamblingMachine;
    
    [SerializeField] private GameEvent<GameEventInfo> _fullSkullEvent;

    private void Awake()
    {
        _gamblingMachine = GetComponent<GL_BaseGamblingMachine>();
        
        _fullSkullEvent?.AddListener(OnFullSkullEvent);
    }

    private void OnFullSkullEvent(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }
        GameObject lastPlayer = _gamblingMachine.LastPlayer;
        lastPlayer.TryGetComponentInParents(out GL_ICoinHolder playerCoinHolder);

        float removeAmount = 15;
        float moneyInserted = _gamblingMachine.CoinHolder.MoneyInserted;
    
        float removedFromMachine = Math.Min(moneyInserted, removeAmount);
        _gamblingMachine.CoinHolder.RemoveMoney(removedFromMachine);

        float remainingAmount = removeAmount - removedFromMachine;
        if (remainingAmount > 0)
        {
            playerCoinHolder.RemoveMoney(remainingAmount);
        }
    }
}
