using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtension
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public static bool TryGetComponentInParents<T>(this GameObject gameObject, out T component)
        {
            component = gameObject.GetComponentInParent<T>();
            if (component != null)
            {
                return true;
            }
            return false;
        }
        
        public static Bounds GetCollidersBounds(this GameObject gameObject)
        {
            var bounds = new Bounds(gameObject.transform.position, Vector3.zero);
            foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
            {
                bounds.Encapsulate(collider.bounds);
            }
            return bounds;
        }
    }
}