using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtension
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static bool TryGetComponentInParents<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInParent<T>(true);
            if (component != null)
            {
                return true;
            }
            return false;
        }
        
        public static Bounds GetCollidersBounds(this GameObject gameObject, bool ignoreDisabled = false)
        {
            var bounds = new Bounds(gameObject.transform.position, Vector3.zero);
            bool shouldDisableGameObject = false;
            if (!gameObject.activeSelf)
            {
                shouldDisableGameObject = true;
                gameObject.SetActive(true);
                Physics.SyncTransforms();
            }
            
            foreach (var collider in gameObject.GetComponentsInChildren<Collider>(true))
            {
                bool disableCollGameobject = false;
                if (!collider.gameObject.activeSelf && !ignoreDisabled)
                {
                    disableCollGameobject = true;
                    collider.gameObject.SetActive(true);
                    Physics.SyncTransforms();
                }
                
                bool disableColl = false;
                if (!collider.enabled && !ignoreDisabled)
                {
                    disableColl = true;
                    collider.enabled = true;
                    Physics.SyncTransforms();
                }
                
                collider.transform.position += Vector3.one;
                Physics.SyncTransforms();
                collider.transform.position -= Vector3.one;
                Physics.SyncTransforms();
                
                bounds.Encapsulate(collider.bounds);
                
                //si on ne fais pas d'incantation magique dans cet ordre précis
                //les bounds sont de (0, 0, 0) alors que là ça fonctionne, tkt tkt

                if (disableColl)
                {
                    collider.enabled = false;
                }

                if (disableCollGameobject)
                {
                    collider.gameObject.SetActive(false);
                }
            }

            if (shouldDisableGameObject)
            {
                gameObject.SetActive(false);
            }
            return bounds;
        }
    }
}