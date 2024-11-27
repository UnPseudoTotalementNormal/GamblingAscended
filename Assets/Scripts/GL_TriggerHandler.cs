using System;
using System.Collections.Generic;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

namespace NavmeshTools
{
    public class GL_TriggerHandler : MonoBehaviour
    {
        private List<Collider> _triggerList = new();

        private int[] _objectId;
        
        private void Awake()
        {
            _objectId = new [] { gameObject.GetGameID() };
        }

        private void OnTriggerEnter(Collider other)
        {
            _triggerList.Add(other);
            var eventInfo = new GameEventTriggerHandler
            {
                Ids = _objectId,
                TriggerValue = other,
                TriggerList = _triggerList,
            };
            GameEventEnum.OnTriggerEnter.Invoke(eventInfo);
            GameEventEnum.OnTriggerUpdate.Invoke(eventInfo);
        }

        private void OnTriggerExit(Collider other)
        {
            _triggerList.Remove(other);
            var eventInfo = new GameEventTriggerHandler
            {
                Ids = _objectId,
                TriggerValue = other,
                TriggerList = _triggerList,
            };
            GameEventEnum.OnTriggerExit.Invoke(eventInfo);
            GameEventEnum.OnTriggerUpdate.Invoke(eventInfo);
        }
    }
}