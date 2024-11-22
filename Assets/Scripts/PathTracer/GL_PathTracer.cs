using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class GL_PathTracer : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    [SerializeField] private SpriteShape _spriteShape;
    [SerializeField] private Material _spriteMaterial;

    public SerializedDictionary<float, Vector3> Waypoints = new();
    
    private void Awake()
    {
        TracePath();
    }

    private void TracePath()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
        NavMeshPath path = new();
        if (!TryGetPathTo(_startTransform.position, _endTransform.position, ref path))
        {
            Debug.LogWarning("No path found");
            return;
        }
        
        SetPath(path);
    }

    private void SetPath(NavMeshPath path)
    {
        var newPath = new GameObject("Path");
        Transform pathTransform = newPath.transform;
        pathTransform.SetParent(transform);
        pathTransform.Rotate(Vector3.right * 90);
        
        var newSpriteShapeController = newPath.AddComponent<SpriteShapeController>();
        newSpriteShapeController.spriteShape = _spriteShape;
        newSpriteShapeController.splineDetail = 16;
        
        var spriteShapeRenderer = pathTransform.GetComponent<SpriteShapeRenderer>();
        List<Material> materials = new()
        {
            _spriteMaterial,
            _spriteMaterial
        };
        spriteShapeRenderer.SetMaterials(materials);
        
        Spline spriteSpline = newSpriteShapeController.spline;
        spriteSpline.isOpenEnded = true;

        float totalDistance = 0;
        for (int i = 0; i < path.corners.Length ; i++)
        {
            Vector3 cornerPos = path.corners[i];
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);
            Vector3 rotatedVector = rotation * cornerPos;
            
            spriteSpline.InsertPointAt(i, rotatedVector);
            spriteSpline.SetTangentMode(i, ShapeTangentMode.Continuous);

            float distance = 0;
            if (i > 0)
            {
                distance = Vector3.Distance(path.corners[i - 1], path.corners[i]);
                totalDistance += distance;
            }
            Waypoints[totalDistance] = cornerPos;
        }
    }

    public bool TryGetPathTo(Vector3 startPos, Vector3 targetPos, ref NavMeshPath path)
    {
        return NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path) && path.corners.Length > 1;
    }
}
