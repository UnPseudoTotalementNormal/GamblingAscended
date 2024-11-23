using UnityEngine;

namespace Interactables.ObjectHolding_Placing
{
    public interface GL_IPlaceable
    {
        public void OnPlaced();
        public bool CanBePlacedAt(Vector3 position);
    }
}