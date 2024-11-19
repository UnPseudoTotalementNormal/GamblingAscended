using System;
using GamblingScripts.BasicComponents;
using GameEvents;
using UnityEngine;

public interface GL_IGamblingMachine
{
    public GameEvent<GameEventInfo> PlayMachineEvent { get; }
    
    public GL_CoinHolder CoinHolder { get; }
    
    public float PlayMoneyCost { get; }

    public float GetMoneyInserted();

    public bool TryPlay();

    public void Play();
}
