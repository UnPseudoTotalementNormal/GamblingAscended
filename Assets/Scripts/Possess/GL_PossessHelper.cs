using Extensions;
using GameEvents;
using UnityEngine;

namespace Possess
{
    public static class GL_PossessHelper
    {
        #region Possess

            public static void Possess(this GameObject gameObject, bool goToGameIdRoot = true) => gameObject.transform.Possess(goToGameIdRoot);
            
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
            
        #endregion

        #region UnPossess
        
            public static void UnPossess(this GameObject gameObject, bool goToGameIdRoot = true) => gameObject.transform.UnPossess(goToGameIdRoot);
            
            public static void UnPossess(this Transform transform, bool goToGameIdRoot = true)
            {
                Transform possessingTransform = transform;

                if (goToGameIdRoot && transform.TryGetComponentInParents(out GameID gameID))
                {
                    possessingTransform = gameID.transform;
                }
                
                TryUnPossessChilds(possessingTransform);
            }

            private static void TryUnPossessChilds(Transform transform)
            {
                TryUnPosses(transform);
                foreach (Transform child in transform)
                {
                    TryUnPossessChilds(child);
                }
            }

            private static void TryUnPosses(Transform transform)
            {
                Component[] possessableComponents = transform.GetComponents(typeof(GL_IPossessable));
                foreach (Component possessable in possessableComponents)
                {
                    (possessable as GL_IPossessable)?.Unpossess();
                }
            }

        #endregion
    }
}