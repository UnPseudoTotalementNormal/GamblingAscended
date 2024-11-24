using UnityEngine;

namespace Interactables.ObjectHolding_Placing
{
    public interface GL_IPlaceable
    {
        public GameObject PlaceableObject { get; }
        public bool DestroyItemOnPlaced { get; }
        public void OnPlaced();
        public void Place(Vector3 position, Vector3 rotation);
        public bool CanBePlacedAt(Vector3 position);
    }
}