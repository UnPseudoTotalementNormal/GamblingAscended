using System;
using DG.Tweening;
using Interactables;
using UnityEngine;

public class GL_ButtonPush : MonoBehaviour
{
    private new Transform transform;
    
    private Vector3 _defaultLocalPosition;
    [SerializeField] private Vector3 _pushOffset;
    
    private void Awake()
    {
        transform = GetComponent<Transform>();
        
        _defaultLocalPosition = transform.localPosition;
        
        var collider = GetComponent<GL_InteractableCollider>();
        collider.OnInteractAction += PushButton;
    }

    private void PushButton()
    {
        transform.DOKill();
        transform.localPosition = _defaultLocalPosition;
        transform.DOLocalMove(_defaultLocalPosition + _pushOffset, 0.25f)
            .SetEase(Ease.OutQuint)
            .OnComplete(() =>
        {
            transform.DOLocalMove(_defaultLocalPosition, 0.25f);
        });
    }
}
