using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GL_PathTracer : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    [SerializeField] private Sprite _sprite;

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
        
        for (int i = 0; i < path.corners.Length - 1 ; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.magenta, 5f);

            Vector3 direction = (path.corners[i + 1] - path.corners[i]).normalized;
            float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
            
            var newPath = new GameObject();
            var newPathTransform = newPath.transform;
            
            var newSpriteRenderer = newPath.AddComponent<SpriteRenderer>();
            newSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
            newSpriteRenderer.sprite = _sprite;
            newSpriteRenderer.size = new Vector2(_sprite.rect.width / 100f, distance);
            
            newPathTransform.position = path.corners[i] + direction * (distance / 2f);
            newPathTransform.rotation = Quaternion.LookRotation(direction);
            newPathTransform.Rotate(Vector3.right * 90, Space.Self);

            newSpriteRenderer.sortingOrder = i;
        }
    }
    
    public bool TryGetPathTo(Vector3 startPos, Vector3 targetPos, ref NavMeshPath path)
    {
        return NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path) && path.corners.Length > 1;
    }
}
