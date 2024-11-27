using System;
using System.Collections.Generic;
using Enums;
using GameEvents;
using GameEvents.Enum;
using NavmeshTools;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class GL_PathTracer : MonoBehaviour
{
    private const float PATH_SIZE = 2.6f;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    [SerializeField] private SpriteShape _spriteShape;
    [SerializeField] private Material _spriteMaterial;

    [SerializeField] private GameEvent<GameEventInfo> _objectPlacedEvent;
    [SerializeField] private GameEvent<GameEventInfo> _onPathTraced;

    //[SerializeField] private GameEvent<GameEventInfo> _askCanPathTrace;
    //[SerializeField] private GameEvent<GameEventInfo> _anwserCanPathTrace;
    
    public SerializedDictionary<float, Vector3> Waypoints = new();

    private GameObject _pathVisual;
    private GameObject _pathCollider;
    
    private void Awake()
    {
        TracePath();
        _objectPlacedEvent?.AddListener(RetracePath);
        GameEventEnum.AskCanPathTrace.AddListener(AnswerCanPathTrace);
    }

    private void AnswerCanPathTrace(GameEventInfo eventInfo)
    {
        NavMeshPath tryPath = new();
        bool canPathTrace = GL_NavmeshUtils.TryGetPathTo(_startTransform.position, _endTransform.position, ref tryPath);
        
        var answerInfo = new GameEventBool()
        {
            Ids = eventInfo.Ids,
            Sender = gameObject,
            Value = canPathTrace,
        };
        GameEventEnum.AnswerCanPathTrace.Invoke(answerInfo);
    }

    private void RetracePath(GameEventInfo eventInfo)
    {
        TracePath();
    }

    private void TracePath()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
        NavMeshPath path = new();
        if (!GL_NavmeshUtils.TryGetPathTo(_startTransform.position, _endTransform.position, ref path))
        {
            Debug.LogWarning("No path found");
            return;
        }
        
        SetPath(path);
    }

    private void SetPath(NavMeshPath path)
    {
        
        
        SetWaypoints(path);
        SetPathColliders(path);
        SetPathVisuals(path);

        var eventInfo = new GameEventPathTraced()
        {
            Sender = gameObject,
            PathTracerWaypoints = Waypoints,
        };
        _onPathTraced?.Invoke(eventInfo);
    }

    private void SetPathColliders(NavMeshPath path)
    {
        if (_pathCollider)
        {
            Destroy(_pathCollider);
        }
        
        _pathCollider = new GameObject("PathCollider");
        Transform pathColliderTransform = _pathCollider.transform;
        pathColliderTransform.SetParent(transform);
        _pathCollider.layer = LayerMaskEnum.Path.GetLayer();
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            
            Vector3 start = path.corners[i - 1];
            Vector3 end = path.corners[i];
            GameObject newObject = new GameObject("PathCollider");
            newObject.layer = (int)(Math.Log((int)LayerMaskEnum.Path) / Math.Log(2));
            BoxCollider newCollider = newObject.AddComponent<BoxCollider>();
            var newColliderTransform = newObject.transform;
            newColliderTransform.SetParent(pathColliderTransform);
            newColliderTransform.position = Vector3.Lerp(start, end, 0.5f);
            newColliderTransform.LookAt(end);
            newColliderTransform.Rotate(Vector3.up * 90);
            newCollider.size = new Vector3(Vector3.Distance(start, end), PATH_SIZE, PATH_SIZE);
        }
    }

    private void SetPathVisuals(NavMeshPath path)
    {
        if (_pathVisual)
        {
            Destroy(_pathVisual);
        }
        
        _pathVisual = new GameObject("PathVisuals");
        Transform pathVisualTransform = _pathVisual.transform;
        pathVisualTransform.SetParent(transform);
        pathVisualTransform.Rotate(Vector3.right * 90);
        
        var newSpriteShapeController = _pathVisual.AddComponent<SpriteShapeController>();
        newSpriteShapeController.spriteShape = _spriteShape;
        newSpriteShapeController.splineDetail = 16;
        
        var spriteShapeRenderer = pathVisualTransform.GetComponent<SpriteShapeRenderer>();
        List<Material> materials = new()
        {
            _spriteMaterial,
            _spriteMaterial
        };
        spriteShapeRenderer.SetMaterials(materials);
        
        Spline spriteSpline = newSpriteShapeController.spline;
        spriteSpline.isOpenEnded = true;
        
        for (int i = 0; i < path.corners.Length ; i++)
        {
            Vector3 cornerPos = path.corners[i];
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);
            Vector3 rotatedVector = rotation * cornerPos;
            
            spriteSpline.InsertPointAt(i, rotatedVector);
            spriteSpline.SetTangentMode(i, ShapeTangentMode.Continuous);
        }
    }

    private void SetWaypoints(NavMeshPath path)
    {
        Waypoints.Clear();
        float totalDistance = 0;
        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 cornerPos = path.corners[i];

            if (i > 0)
            {
                float distance = Vector3.Distance(path.corners[i - 1], path.corners[i]);
                totalDistance += distance;
            }
            Waypoints[totalDistance] = cornerPos;
        }
    }
}
