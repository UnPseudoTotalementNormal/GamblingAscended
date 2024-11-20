using UnityEngine;

namespace Extensions
{
    public static class TransformExtension
    {
        public static bool TryGetComponentInParents<T>(this Transform transform, out T component)
        {
            return transform.gameObject.TryGetComponentInParents(out component);
        }
    }
}