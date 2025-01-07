using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

namespace Interactables
{
    public class GL_InteractableCollider : MonoBehaviour, GL_IInteract, GL_IInteractable
    {
        public Action OnInteractAction;
        public Action OnPointerEnterAction;
        public Action OnPointerExitAction;
        [field:SerializeField] public GameEventEnum InteractPointerEnterEvent { get; private set; } = GameEventEnum.InteractPointerEnter;

        [field: SerializeField] public GameEventEnum InteractPointerExitEvent { get; private set; } = GameEventEnum.InteractPointerExit;

        [field: SerializeField] public GameEventEnum InteractionEvent { get; private set; } = GameEventEnum.NoneEvent;
        

        public void OnEnter()
        {
            var eventInfo = new GameEventGameObject
            {
                Value = gameObject,
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            InteractPointerEnterEvent.Invoke(eventInfo);
            OnPointerEnterAction?.Invoke();
        }

        public void OnExit()
        {
            if (!this) //check if object is destroyed
            {
                var nullEventInfo = new GameEventGameObject
                {
                    Ids = new[] { -1 },
                };
                InteractPointerExitEvent.Invoke(nullEventInfo);
                OnPointerExitAction?.Invoke();
                return;
            }
            var eventInfo = new GameEventGameObject
            {
                Value = gameObject,
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            InteractPointerExitEvent.Invoke(eventInfo);
            OnPointerExitAction?.Invoke();
        }

        public void OnInteract(GameObject sender)
        {
            var eventInfo = new GameEventGameObject()
            {
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
                Value = sender,
            };
            InteractionEvent.Invoke(eventInfo);
            OnInteractAction?.Invoke();
        }
    }
}