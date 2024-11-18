using System;
using GameEvents;
using UnityEngine;

namespace Interactables
{
    public class GL_InteractableCollider : MonoBehaviour, GL_IInteract, GL_IInteractable
    {
        [field:SerializeField] public GameEvent<GameObject> InteractPointerEnterEvent { get; private set; }
        [field:SerializeField] public GameEvent<GameObject> InteractPointerExitEvent { get; private set; }
        [field:SerializeField] public GameEvent InteractionEvent { get; private set; }
        

        public void OnEnter()
        {
            InteractPointerEnterEvent.Invoke(gameObject, gameObject.GetGameID());
        }

        public void OnExit()
        {
            InteractPointerExitEvent.Invoke(gameObject, gameObject.GetGameID());
        }

        public void OnInteract()
        {
            InteractionEvent.Invoke(gameObject.GetGameID());
        }
    }
}