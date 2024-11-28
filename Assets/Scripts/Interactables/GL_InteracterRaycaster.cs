using System;
using Enums;
using GameEvents;
using Possess;
using UnityEngine;

namespace Interactables
{
    public class GL_InteracterRaycaster : MonoBehaviour, GL_IPossessable
    {
        private Transform _transform;
        private GameObject _owner;
        public bool IsPossessed { get; private set; }
        public GameEvent<GameEventInfo> OnPossessedEvent { get; }
        public GameEvent<GameEventInfo> OnUnpossessedEvent { get; }

        [SerializeField] private GameEvent<GameEventInfo> _interactInputEvent;

        [SerializeField] private float _raycastLength;

        private GL_IInteractable _currentInteractable;
        private GL_IInteractable _oldInteractable;
        private bool _isOnInteractable;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _owner = gameObject;
        }

        private void Update()
        {
            if (CheckInteract())
            {
                if (_currentInteractable == _oldInteractable && _isOnInteractable == true) return;
                
                OldInteractableExit();
                
                _currentInteractable?.OnEnter();
                _oldInteractable = _currentInteractable;
                _isOnInteractable = true;
            }
            else
            {
                if (_isOnInteractable == false) return;
                
                OldInteractableExit();
                _isOnInteractable = false;
            }
        }
        
        private void OldInteractableExit()
        {
            if (_oldInteractable == null) return;
            
            _oldInteractable?.OnExit();
            _oldInteractable = null;
        }

        private bool CheckInteract()
        {
            if (!TryGetInteractable(out GL_IInteractable interactable))
            {
                _currentInteractable = null;
                return false;
            }

            _currentInteractable = interactable;

            return true;
        }

        private void TryInteract(GameEventInfo eventInfo)
        {
            if (!enabled || (_currentInteractable == null && !TryGetInteractable(out _currentInteractable)))
            {
                return;
            }
            
            _currentInteractable.OnInteract(_owner);
        }
        
        private bool TryGetInteractable(out GL_IInteractable interactable)
        {
            Ray ray = new Ray(_transform.position, _transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, _raycastLength, 
                    ~((int)LayerMaskEnum.Path | (int)LayerMaskEnum.IgnoreRaycast)))
            {
                interactable = null;
                return false;
            }
            
            return hitInfo.collider.TryGetComponent<GL_IInteractable>(out interactable);
        }

        public void EnableComponent()
        {
            enabled = true;
        }

        public void DisableComponent()
        {
            _currentInteractable = null;
            OldInteractableExit();
            enabled = false;
        }

        void GL_IPossessable.OnPossess()
        {
            _interactInputEvent.AddListener(TryInteract);
            OnPossessedEvent?.Invoke(new GameEventGameObject {Value = gameObject});
            IsPossessed = true;
        }

        void GL_IPossessable.OnUnpossess()
        {
            _interactInputEvent.RemoveListener(TryInteract);
            OnUnpossessedEvent?.Invoke(new GameEventGameObject {Value = gameObject});
            IsPossessed = false;
        }
    }
}