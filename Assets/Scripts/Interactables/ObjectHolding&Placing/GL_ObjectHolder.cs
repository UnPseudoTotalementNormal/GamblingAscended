using System;
using GameEvents;
using Interactables;
using Interactables.ObjectHolding_Placing;
using UnityEngine;

[RequireComponent(typeof(GL_InteracterRaycaster))]
public class GL_ObjectHolder : MonoBehaviour
{
    private GL_InteracterRaycaster _interacterRaycaster;
    private GL_IHoldable _currentHoldable;
    
    [SerializeField] private GameEvent<GameEventInfo> _tryPickupEvent;
    [SerializeField] private GameEvent<GameEventInfo> _interactInputEvent;


    private void Awake()
    {
        _interacterRaycaster = GetComponent<GL_InteracterRaycaster>();
        _tryPickupEvent?.AddListener(OnTryPickup);
        _interactInputEvent?.AddListener(TryDrop);
    }
    

    public void OnTryPickup(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
        {
            return;
        }
        
        Pickup(gameEventGameObject.Value);
    }
    
    public void Pickup(GameObject pickedObject)
    {
        if (!pickedObject.TryGetComponent(out GL_IHoldable holdable))
        {
            return;
        }
        
        _currentHoldable = holdable;
        _currentHoldable.OnPickup();
        
        _interacterRaycaster.DisableComponent();
    }

    public void Drop()
    {
        if (_currentHoldable == null)
        {
            return;
        }
        
        _interacterRaycaster.EnableComponent();
        _currentHoldable = null;
    }
    
    private void TryDrop(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || _currentHoldable == null)
        {
            return;
        }
        
        Drop();
    }
}
