using System;

public interface GL_IGamblingMachine
{
    public event Action PlayMachineEvent;
    
    public float PlayMoneyCost { get; }

    public float GetMoneyInserted();

    public bool TryPlay();

    public void Play();
}
