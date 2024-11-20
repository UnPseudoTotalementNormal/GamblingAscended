using System;
using Possess;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GL_PossessableCamera : MonoBehaviour, GL_IPossessable
{
    private Camera _camera;
    private AudioListener _audioListener;
    public bool IsPossessed { get; private set; }
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        TryGetComponent(out _audioListener);
        DeactivateCamera();
    }


    void GL_IPossessable.OnPossess()
    {
        ActivateCamera();
        IsPossessed = true;
    }

    void GL_IPossessable.OnUnpossess()
    {
        DeactivateCamera();
        IsPossessed = false;
    }

    private void ActivateCamera()
    {
        _camera.depth = 10;
        if (_audioListener)
        {
            _audioListener.enabled = true;
        }
    }

    private void DeactivateCamera()
    {
        _camera.depth = -10;
        if (_audioListener)
        {
            _audioListener.enabled = false;
        }
    }
}
