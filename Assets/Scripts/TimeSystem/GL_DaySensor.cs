using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_DaySensor : MonoBehaviour
{
    [SerializeField] private GL_TimeManager.StateOfDay TurnOffTime = GL_TimeManager.StateOfDay.Night;
    
    private void Awake()
    {
        GameEventEnum.OnDayEnded.AddListener((info => { OnNight(); }));
        GameEventEnum.OnNightEnded.AddListener((info => { OnDay(); }));
    }

    private void OnNight()
    {
        if (TurnOffTime == GL_TimeManager.StateOfDay.Night)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    private void OnDay()
    {
        if (TurnOffTime == GL_TimeManager.StateOfDay.Day)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    private void TurnOff()
    {
        GameEventEnum.TurnOff.Invoke(new GameEventInfo {Ids = new [] { gameObject.GetGameID() }});
    }

    private void TurnOn()
    {
        GameEventEnum.TurnOn.Invoke(new GameEventInfo {Ids = new [] { gameObject.GetGameID() }});
    }
}
