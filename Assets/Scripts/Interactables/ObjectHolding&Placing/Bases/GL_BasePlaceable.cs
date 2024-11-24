using System;
using Enums;
using Extensions;
using GameEvents;
using UnityEngine;

namespace Interactables.ObjectHolding_Placing.Bases
{
    [RequireComponent(typeof(GL_IHoldable))]
    public class GL_BasePlaceable : MonoBehaviour, GL_IPlaceable
    {
        private Collider[] _tryPlaceResults = new Collider[50];
        [field:SerializeField] public GameObject PlaceableObject { get; private set; }
        [field: SerializeField] public bool DestroyItemOnPlaced { get; private set; } = true;
        [SerializeField] private GameEvent<GameEventInfo> _objectPlacedEvent;
        
        public void OnPlaced()
        {
            
        }

        public void Place(Vector3 position, Vector3 rotation)
        {
            var newObject = Instantiate(PlaceableObject, position, Quaternion.Euler(rotation));

            var eventInfo = new GameEventInfo()
            {
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            _objectPlacedEvent?.Invoke(eventInfo);
        }

        public bool CanBePlacedAt(Vector3 position)
        {
            Bounds objectBounds = gameObject.GetCollidersBounds();
            var size = Physics.OverlapBoxNonAlloc(position + objectBounds.center, objectBounds.extents, _tryPlaceResults,
                Quaternion.identity, ~(int)LayerMaskEnum.IgnorePlaceable);
            if (size == 0)
            {
                return true;
            }
            
            return false;
        }
        
        [ContextMenu("Print Bounds")]
        public void PrintBounds()
        {
            var bounds = gameObject.GetCollidersBounds();
            Debug.Log($"Center: {bounds.center}, Extents: {bounds.extents}");
        }
    }
}