using System;
using Enums;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using TMPro;
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
        private int _amount = 1;
        public int Amount
        {
            get { return _amount;}
            set
            {
                _amount = value;
                RefreshAmountText();
            }
        }

        [SerializeField] private TextMeshPro _amountText;

        protected virtual void Start()
        {
            RefreshAmountText();
        }

        private void RefreshAmountText()
        {
            _amountText.text = Amount.ToString();
        }
        
        public virtual void OnPlaced(GameObject spawnedObject)
        {
            Amount -= 1;
            if (Amount <= 0)
            {
                GameEventEnum.InteractInputStarted.Invoke(new GameEventInfo());
                Destroy(gameObject);
            }
        }

        public GameObject Place(Vector3 position, Vector3 rotation)
        {
            var newObject = Instantiate(PlaceableObject, position, Quaternion.Euler(rotation));

            var eventInfo = new GameEventInfo()
            {
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            OnPlaced(newObject);
            _objectPlacedEvent?.Invoke(eventInfo);
            return newObject;
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
    }
}