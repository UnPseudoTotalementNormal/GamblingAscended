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
    }
}