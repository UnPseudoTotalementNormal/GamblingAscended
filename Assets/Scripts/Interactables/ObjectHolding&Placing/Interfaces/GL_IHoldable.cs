#nullable enable
using UnityEngine;

namespace Interactables.ObjectHolding_Placing
{
    public interface GL_IHoldable
    {
        public GL_IPlaceable GetPlaceable();
        public bool IsPlaceable();
        public GameObject GetGameObject();
        
        public void OnPickup();
        public void OnDropped();
    }
}