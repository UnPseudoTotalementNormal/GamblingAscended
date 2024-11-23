using Enums;
using Extensions;
using UnityEngine;

namespace Interactables.ObjectHolding_Placing.Bases
{
    [RequireComponent(typeof(GL_IHoldable))]
    public class GL_BasePlaceable : MonoBehaviour, GL_IPlaceable
    {
        private Collider[] _tryPlaceResults = new Collider[50];
        
        public void OnPlaced()
        {
            
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
    }
}