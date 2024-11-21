using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GL_PathTracer : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    private void Awake()
    {
        TracePath();
    }

    [ContextMenu("TracePath")]
    private void TracePath()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
        NavMeshPath path = new();
        if (!TryGetPathTo(_startTransform.position, _endTransform.position, ref path))
        {
            return;
        }
        
        for (int i = 1; i < path.corners.Length; i++)
        {
            Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.magenta, 5f);
        }
        
        Debug.Break();
    }
    
    public bool TryGetPathTo(Vector3 startPos, Vector3 targetPos, ref NavMeshPath path)
    {
        return NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path) && path.corners.Length > 1;
    }
}
