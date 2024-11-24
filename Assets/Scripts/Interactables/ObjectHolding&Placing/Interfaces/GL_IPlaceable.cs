using UnityEngine;

namespace Interactables.ObjectHolding_Placing
{
    public interface GL_IPlaceable
    {
        public GameObject PlaceableObject { get; }
        public void OnPlaced();
        public bool CanBePlacedAt(Vector3 position);
    }
}