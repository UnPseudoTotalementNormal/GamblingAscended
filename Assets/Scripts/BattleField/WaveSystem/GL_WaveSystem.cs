using System;
using System.Collections.Generic;
using System.Linq;
using BattleField.WaveSystem;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;
using static BattleField.WaveSystem.GL_WaveInfo;

public class GL_WaveSystem : MonoBehaviour
{
    public int CurrentWave = 0;
    
    [SerializeField] private List<GL_WaveInfo> _waves = new();
    private bool _isWaveRunning = false;

    private float _waveTimer = 0;

    private List<GL_EnemySpawner> _enemySpawners = new();
    
    private List<EnemySpawner> _currentWaveInfo;
    
    private void Awake()
    {
        GetSpawners();
        StartWave();
    }

    private void GetSpawners()
    {
        GL_EnemySpawner[] spawners = GetComponentsInChildren<GL_EnemySpawner>(true);
        foreach (GL_EnemySpawner spawner in spawners)
        {
            _enemySpawners.Add(spawner);
        }
    }

    private void StartWave()
    {
        _currentWaveInfo = _waves[CurrentWave].SpawnInfo.ToList();
        GameEventEnum.OnWaveStarted.Invoke(new GameEventInfo());
    }

    private void Update()
    {
        _waveTimer += Time.deltaTime;

        for (var i = 0; i < _currentWaveInfo.Count; i++)
        {
            EnemySpawner enemySpawner = _currentWaveInfo[i];
            bool shouldSpawn = enemySpawner.ShouldSpawn(_waveTimer);
            _currentWaveInfo[i] = enemySpawner;
            
            if (!shouldSpawn)
            {
                continue;
            }

            var eventInfo = new GameEventSpawnEnemy()
            {
                EnemyObject = enemySpawner.Infos.Enemy,
                Sender = gameObject
            };

            if (enemySpawner.SpawnedCount >= enemySpawner.Infos.Count)
            {
                _currentWaveInfo.RemoveAt(i);
                i--;
            }

            if (_currentWaveInfo.Count == 0)
            {
                Debug.Log("end wave");
            }
            GameEventEnum.SpawnEnemy.Invoke(eventInfo);
        }
    }
}
