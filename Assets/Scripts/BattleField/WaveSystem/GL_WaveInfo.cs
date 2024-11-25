using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleField.WaveSystem
{
    [CreateAssetMenu(fileName = "WaveInfo", menuName = "WaveInfo")]
    public class GL_WaveInfo : ScriptableObject
    {
        public List<EnemySpawner> SpawnInfo;
        
        [Serializable]
        public struct EnemySpawner
        {
            public int Enemy; //todo: replace with Scriptable
            public int Count;
            public float Interval;
            public float StartTime;
        }
    }
}