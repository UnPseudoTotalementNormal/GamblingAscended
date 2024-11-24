using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Abs(this Vector3 v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        

    }
}