using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace GameEvents
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
            //_Ids.Remove(_id);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static int GetGameID(GameObject gameObject)
        {
            bool hasGameID = gameObject.TryGetComponentInParents<GameID>(out GameID gameID);
            if (!hasGameID)
            {
                Debug.LogError("GameID not found on " + gameObject.name);
                return -1;
            }
            
            return gameID.GetID();
        }

        public static void CleanIds()
        {
            _Ids = new List<int>();
        }
    }
}