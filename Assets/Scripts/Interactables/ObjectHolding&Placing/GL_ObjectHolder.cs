using System;
using Enums;
using Extensions;
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

    [SerializeField] private float _dropMaxDistance = 2;


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

        
        
        var droppedObject = _currentHoldable.GetGameObject();
        droppedObject.SetActive(true); //to get the fucking collider bounds, otherwise it won't work fuck unity
        Bounds objectBounds = droppedObject.GetCollidersBounds();
        droppedObject.SetActive(false);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _dropMaxDistance,
                ~(int)LayerMaskEnum.Character))
        {
            droppedObject.transform.position = hitInfo.point + objectBounds.extents.y * Vector3.up;
            
        }
        else
        {
            droppedObject.transform.position = transform.position + (transform.forward * _dropMaxDistance) -
                                               objectBounds.extents.x * Vector3.right;
        }
        
        _currentHoldable.OnDropped();
        Timer.Timer.NewTimer(0, () => { _interacterRaycaster.EnableComponent(); });
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
