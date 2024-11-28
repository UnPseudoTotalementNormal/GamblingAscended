using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_DeathDestroyer : MonoBehaviour
{
    private void Awake()
    {
        GameEventEnum.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids))
        {
            return;
        }
        
        Destroy(gameObject);
    }
}
