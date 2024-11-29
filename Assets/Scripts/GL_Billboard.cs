using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_Billboard : MonoBehaviour
{
    public bool BillboardX = true;
    public bool BillboardY = true;
    private Camera _currentCamera;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        GameEventEnum.Possess.AddListener((info => {UpdateCamera();}));
    }

    private void Start()
    {
        UpdateCamera();
    }

    private void LateUpdate()
    {
        if (!_currentCamera) return;

        _transform.LookAt(_transform.position + _currentCamera.transform.rotation * Vector3.forward);
    }

    private void UpdateCamera()
    {
        _currentCamera = Camera.main;
    }
}