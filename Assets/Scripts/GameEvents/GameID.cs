using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvent
{
    public class GameID : MonoBehaviour
    {
        private static List<int> _Ids = new();

        private int _id;

        private int GetID() => _id;
        
        private void Awake()
        {
            _id = GenerateNewID();
            _Ids.Add(_id);
        }

        private int GenerateNewID() => _Ids.Count;
        
        private void OnDestroy()
        {
            _Ids.Remove(_id);
        }

        public static int GetObjectID(GameObject gameObject)
        {
            return gameObject.GetComponentInParent<GameID>().GetID();
        }
    }
}