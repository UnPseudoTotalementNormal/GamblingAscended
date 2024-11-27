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

    private bool _isEndingWave = false;
    
    private void Awake()
    {
        GetSpawners();
        GameEventEnum.OnSleep.AddListener(OnSleep);
    }

    private void Start()
    {
        StopWave();
    }
    
    private void OnSleep(GameEventInfo eventInfo)
    {
        Timer.Timer.NewTimer(7, StartWave);
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
        _isEndingWave = false;
        _isWaveRunning = true;
        _currentWaveInfo = _waves[CurrentWave].SpawnInfo.ToList();
        GameEventEnum.OnWaveStarted.Invoke(new GameEventFloat { Value = CurrentWave });
    }
    
    private void StopWave()
    {
        _isWaveRunning = false;
        _isEndingWave = false;
        GameEventEnum.OnWaveEnded.Invoke(new GameEventFloat { Value = CurrentWave });
    }

    private void Update()
    {
        if (!_isWaveRunning)
        {
            return;
        }
        
        _waveTimer += Time.deltaTime;

        if (!_isEndingWave)
        {
            UpdateEnemySpawners();
            return;
        }

        if (CheckEndingWave())
        {
            CurrentWave += 1;
            StopWave();
        }
    }

    private bool CheckEndingWave()
    {
        foreach (GL_EnemySpawner spawner in _enemySpawners)
        {
            if (spawner.GetAliveEnemies().Count != 0)
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateEnemySpawners()
    {
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
                _isEndingWave = true;
            }
            GameEventEnum.SpawnEnemy.Invoke(eventInfo);
        }
    }
}
