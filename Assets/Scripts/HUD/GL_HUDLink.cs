using System;
using GameEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GL_HUDLink : MonoBehaviour
{
    public GameEvent<GameEventInfo> EventLink;

    public UnityEvent<GameObject> OnHUDLinkedWithGameObject; 
    
    private void Awake()
    {
        EventLink.AddListener(Link);
    }

    public void Link(GameEventInfo gameEventInfo)
    {
        if (gameEventInfo.TryTo(out GameEventGameObject gameEventGameObject))
        {
            OnHUDLinkedWithGameObject?.Invoke(gameEventGameObject.Value);
        }
    }
}
