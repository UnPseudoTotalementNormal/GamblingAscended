using System;
using DG.Tweening;
using GameEvents;
using UnityEngine;

public class GL_Lever : MonoBehaviour
{
    [SerializeField] private GameEvent _pullLeverEvent;

    [SerializeField] private Vector3 _baseRotation;
    [SerializeField] private Vector3 _pullRotation;
    [SerializeField] private float _pullDuration;
    [SerializeField] private Ease _pullEase;
    private void Awake()
    {
        _pullLeverEvent.AddListener(OnPullLever);
    }

    private void OnPullLever(int[] ids)
    {
        if (!gameObject.HasGameID(ids))
        {
            return;
        }

        transform.DOLocalRotate(_pullRotation, _pullDuration).SetEase(_pullEase).onComplete += () 
            => transform.DOLocalRotate(_baseRotation, _pullDuration).SetEase(_pullEase);
    }
}
