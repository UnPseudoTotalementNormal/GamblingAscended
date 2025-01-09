using System;
using DG.Tweening;
using Interactables;
using UnityEngine;
using UnityEngine.Serialization;

public class GL_Drawer : MonoBehaviour
{
    private Vector3 _closedPosition; 
    [SerializeField] private Vector3 _openedPosition;
    [SerializeField] private float _animationTime = 0.25f;
    [SerializeField] private Ease _animationEase = Ease.OutQuint;
    
    private void Awake()
    {
        _closedPosition = transform.localPosition;
        var collider = GetComponent<GL_InteractableCollider>();
        collider.OnPointerEnterAction += OpenDrawer;
        collider.OnPointerExitAction += CloseDrawer;
    }

    private void OpenDrawer()
    {
        transform.DOKill();
        transform.DOLocalMove(_openedPosition, 0.25f);
    }
    
    private void CloseDrawer()
    {
        transform.DOKill();
        transform.DOLocalMove(_closedPosition, 0.25f);
    }
}
