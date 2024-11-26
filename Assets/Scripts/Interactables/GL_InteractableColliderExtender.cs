using GameEvents;
using UnityEngine;

namespace Interactables
{
    public class GL_InteractableColliderExtender : MonoBehaviour, GL_IInteract, GL_IInteractable
    {
        public GameEventEnum InteractPointerEnterEvent { get; }
        public GameEventEnum InteractPointerExitEvent { get; }
        public GameEventEnum InteractionEvent { get; }
        
        [Header("If none it will take the first interactableCollider in the parents")]
        [SerializeField] private GL_InteractableCollider _interactableCollider;
        public void OnEnter()
        {
            GetInteractableCollider()?.OnEnter();
        }

        public void OnExit()
        {
            GetInteractableCollider()?.OnExit();
        }

        public void OnInteract(GameObject sender)
        {
            GetInteractableCollider()?.OnInteract(sender);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private GL_InteractableCollider GetInteractableCollider()
        {
            if (_interactableCollider)
            {
                return _interactableCollider;
            }
            _interactableCollider = GetComponentInParent<GL_InteractableCollider>();
            return _interactableCollider;
        }
    }
}