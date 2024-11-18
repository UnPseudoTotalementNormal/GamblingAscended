using GameEvents;
using UnityEngine;

namespace Interactables
{
    public class GL_InteractableCollider : Component, GL_IInteract, GL_IInteractable
    {
        [field:SerializeField] public GameEvent<GameObject> InteractPointerEnterEvent { get; private set; }
        [field:SerializeField] public GameEvent<GameObject> InteractPointerExitEvent { get; private set; }
        [field:SerializeField] public GameEvent InteractionEvent { get; private set; }
        

        public void OnEnter()
        {
            InteractPointerEnterEvent.Invoke(gameObject);
        }

        public void OnExit()
        {
            InteractPointerExitEvent.Invoke(gameObject);
        }

    }
}