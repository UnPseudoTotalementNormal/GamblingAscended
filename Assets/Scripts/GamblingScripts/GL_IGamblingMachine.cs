using System;
using UnityEngine;

public interface GL_IGamblingMachine
{
    public Transform ScreenTransform { get; }
    public event Action PlayMachineEvent;
    
    public float PlayMoneyCost { get; }

    public float GetMoneyInserted();

    public bool TryPlay();

    public void Play();
}
