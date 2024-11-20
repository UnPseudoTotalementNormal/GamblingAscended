using Extensions;
using GameEvents;
using UnityEngine;

namespace Possess
{
    public static class GL_PossessHelper
    {
        public static void Possess(this Transform transform, bool goToGameIdRoot = true)
        {
            Transform possessingTransform = transform;

            if (goToGameIdRoot && transform.TryGetComponentInParents(out GameID gameID))
            {
                possessingTransform = gameID.transform;
            }
            
            TryPossessChilds(possessingTransform);
        }

        private static void TryPossessChilds(Transform transform)
        {
            TryPosses(transform);
            foreach (Transform child in transform)
            {
                TryPossessChilds(child);
            }
        }

        private static void TryPosses(Transform transform)
        {
            Component[] possessableComponents = transform.GetComponents(typeof(GL_IPossessable));
            foreach (Component possessable in possessableComponents)
            {
                (possessable as GL_IPossessable)?.Possess();
            }
        }
    }
}