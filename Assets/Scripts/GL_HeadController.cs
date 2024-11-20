using System;
using GameEvents;
using Possess;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_HeadController : MonoBehaviour, GL_IPossessable
{
    private Transform _transform;
    public bool IsPossessed { get; private set; }
    
    [Header("Input")]
    [SerializeField] private GameEvent<GameEventInfo> _mouseInputEvent;
    [SerializeField] private float _sensitivity = 1;
    
    [Header("Controls")]
    [SerializeField] private bool _controlXRotation = true;
    [SerializeField] private bool _controlYRotation = true;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnMouseMoved(GameEventInfo eventInfo)
    {
        if (!eventInfo.TryTo(out GameEventVector2 gameEventVector2))
        {
            return;
        }

        Vector2 moveInput = gameEventVector2.Value;
        moveInput *= _sensitivity;
        if (!_controlXRotation) moveInput.y = 0;
        if (!_controlYRotation) moveInput.x = 0;
        
        _transform.Rotate(-moveInput.y, moveInput.x, 0);
        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, 0);
    }

    void GL_IPossessable.OnPossess()
    {
        _mouseInputEvent.AddListener(OnMouseMoved);
        IsPossessed = true;
    }

    void GL_IPossessable.OnUnpossess()
    {
        _mouseInputEvent.RemoveListener(OnMouseMoved);
        IsPossessed = false;
    }
}
