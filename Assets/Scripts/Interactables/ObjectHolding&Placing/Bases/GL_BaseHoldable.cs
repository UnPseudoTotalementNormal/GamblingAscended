using GameEvents;
using UnityEngine;

namespace Interactables.ObjectHolding_Placing.Bases
{
    public class GL_BaseHoldable : MonoBehaviour, GL_IHoldable
    {
        public GameObject GetGameObject() => gameObject;

        public GL_IPlaceable GetPlaceable() => GetComponent<GL_IPlaceable>();
        public bool IsPlaceable() => GetPlaceable() != null;


        public void OnPickup()
        {
            gameObject.SetActive(false);
        }

        public void OnDropped()
        {
            gameObject.SetActive(true);
        }
    }
}