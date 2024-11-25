using System;
using System.Collections.Generic;
using Character.Enemy;
using UnityEngine;

namespace BattleField.WaveSystem
{
    [CreateAssetMenu(fileName = "WaveInfo", menuName = "WaveInfo")]
    public class GL_WaveInfo : ScriptableObject
    {
        public List<EnemySpawner> SpawnInfo;
        
        [Serializable]
        public struct EnemySpawnerInfo
        {
            public GL_EnemyObject Enemy; //todo: replace with Scriptable
            public int Count;
            public float Interval;
            public float StartTime;
        }
        
        [Serializable]
        public struct EnemySpawner
        {
            public EnemySpawnerInfo Infos;

            [HideInInspector] public int SpawnedCount;
            [HideInInspector] public float NextSpawnAtTime;
            
            public bool ShouldSpawn(float waveTime)
            {
                if (Infos.StartTime > waveTime || SpawnedCount >= Infos.Count)
                {
                    return false;
                }

                if (NextSpawnAtTime > waveTime)
                {
                    return false;
                }

                NextSpawnAtTime = waveTime + Infos.Interval;
                SpawnedCount += 1;
                return true;
            }
        }
    }
}