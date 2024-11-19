using System;
using GameEvents;
using UnityEngine;

namespace Interactables
{
    public class GL_InteractableCollider : MonoBehaviour, GL_IInteract, GL_IInteractable
    {
        [field:SerializeField] public GameEvent<GameEventInfo> InteractPointerEnterEvent { get; private set; }
        [field:SerializeField] public GameEvent<GameEventInfo> InteractPointerExitEvent { get; private set; }
        [field:SerializeField] public GameEvent<GameEventInfo> InteractionEvent { get; private set; }
        

        public void OnEnter()
        {
            var eventInfo = new GameEventGameObject
            {
                Value = gameObject,
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            InteractPointerEnterEvent?.Invoke(eventInfo);
        }

        public void OnExit()
        {
            var eventInfo = new GameEventGameObject
            {
                Value = gameObject,
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            InteractPointerExitEvent?.Invoke(eventInfo);
        }

        public void OnInteract(GameObject sender)
        {
            var eventInfo = new GameEventGameObject
            {
                Value = sender,
                Ids = new[] { gameObject.GetGameID() },
                Sender = gameObject,
            };
            InteractionEvent?.Invoke(eventInfo);
        }
    }
}