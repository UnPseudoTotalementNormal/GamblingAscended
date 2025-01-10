using System;
using Character.Enemy;
using DG.Tweening;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_DamagePunch : MonoBehaviour
{
    private void Awake()
    {
        GameEventEnum.OnDamageTaken.AddListener(OnDamageTaken);
    }

    private void OnDamageTaken(GameEventInfo obj)
    {
        if (!obj.TryTo(out GameEventDamage damageInfo))
        {
            return;
        }

        if (damageInfo.Sender.TryGetComponent(out GL_PathFollower pathFollower))
        {
            var pathFollowerModel = pathFollower.Model;
            pathFollowerModel.DOKill(true);
            pathFollowerModel.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 0.3f);
        }
        
    }
}
