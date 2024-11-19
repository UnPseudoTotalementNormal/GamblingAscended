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
            var gameEventInfo = new GameEventGameObject
            {
                Value = gameObject,
            };
            InteractPointerEnterEvent?.Invoke(gameEventInfo, gameObject.GetGameID());
        }

        public void OnExit()
        {
            var gameEventInfo = new GameEventGameObject
            {
                Value = gameObject,
            };
            InteractPointerExitEvent?.Invoke(gameEventInfo, gameObject.GetGameID());
        }

        public void OnInteract(GameObject sender)
        {
            var gameEventInfo = new GameEventGameObject
            {
                Value = sender,
            };
            InteractionEvent?.Invoke(gameEventInfo, gameObject.GetGameID());
        }
    }
}