using System;
using GameEvents;
using UnityEngine;

public class GL_Lever : MonoBehaviour
{
    [SerializeField] private GameEvent _pullLeverEvent;

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
    }
}
