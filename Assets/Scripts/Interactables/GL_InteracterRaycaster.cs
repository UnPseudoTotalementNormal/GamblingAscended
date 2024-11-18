using System;
using GameEvents;
using UnityEngine;

namespace Interactables
{
    public class GL_InteracterRaycaster : MonoBehaviour
    {
        private Transform _transform;

        [SerializeField] private float _raycastLength;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TryInteract();
            }
        }

        private void TryInteract()
        {
            Ray ray = new Ray(_transform.position, _transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, _raycastLength)) return;
            
            if (!hitInfo.collider.TryGetComponent<GL_IInteractable>(out GL_IInteractable interactable)) return;
            
            interactable.OnInteract();
        }
    }
}