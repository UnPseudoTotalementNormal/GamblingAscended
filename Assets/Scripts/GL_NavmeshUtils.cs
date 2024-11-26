using UnityEngine;
using UnityEngine.AI;

namespace NavmeshTools
{
    public static class GL_NavmeshUtils
    {
        public static bool TryGetPathTo(Vector3 startPos, Vector3 targetPos, ref NavMeshPath path)
        {
            return NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path) && path.corners.Length > 1 &&
                   path.status == NavMeshPathStatus.PathComplete;
        }
    }
}