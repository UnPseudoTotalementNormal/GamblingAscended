using UnityEngine;

public class GL_FadeMaterialUpdater : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    void LateUpdate()
    {
        if (!_transform)
        {
            _transform = GetComponent<Transform>();
        }

        _meshRenderer.material.SetVector("_WorldStartPoint", _transform.position);
    }
}
