using System;
using GameEvents;
using UnityEngine;

namespace Interactables.ObjectHolding_Placing.Bases
{
    public class GL_BaseHoldable : MonoBehaviour, GL_IHoldable
    {
        public GameObject GetGameObject() => gameObject;

        public GL_IPlaceable GetPlaceable() => GetComponent<GL_IPlaceable>();
        public bool IsPlaceable() => GetPlaceable() != null;
        
        [SerializeField] private GameEvent<GameEventInfo> _interactionEvent;
        [SerializeField] private GameEvent<GameEventInfo> _tryPickupEvent;
        
        private void Awake()
        {
            _interactionEvent?.AddListener(TryPickup);
        }

        private void TryPickup(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
            {
                return;
            }

            var sendEventInfo = new GameEventGameObject()
            {
                Ids = new [] { gameEventGameObject.Value.GetGameID() },
                Sender = gameObject,
                Value = gameObject
            };
            _tryPickupEvent?.Invoke(sendEventInfo);
        }

        public void OnPickup()
        {
            gameObject.SetActive(false);
        }

        public void OnDropped()
        {
            gameObject.SetActive(true);
            if (TryGetComponent(out Rigidbody body))
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }
    }
}