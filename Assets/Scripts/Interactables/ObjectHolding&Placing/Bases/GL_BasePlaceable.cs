using System;
using Enums;
using Extensions;
using GameEvents;
using UnityEngine;

namespace Interactables.ObjectHolding_Placing.Bases
{
    [RequireComponent(typeof(GL_IHoldable))]
    public class GL_BasePlaceable : MonoBehaviour, GL_IPlaceable
    {
        private Collider[] _tryPlaceResults = new Collider[50];
        
        public void OnPlaced()
        {
            if (TryGetComponent(out Rigidbody body))
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
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
        
        [ContextMenu("Print Bounds")]
        public void PrintBounds()
        {
            var bounds = gameObject.GetCollidersBounds();
            Debug.Log($"Center: {bounds.center}, Extents: {bounds.extents}");
        }
    }
}