using System;
using System.Collections.Generic;
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
    
    private List<EnemySpawner> _currentWaveInfo;
    
    private void Awake()
    {
        StartWave();
    }

    private void StartWave()
    {
        _currentWaveInfo = _waves[CurrentWave].SpawnInfo;
        GameEventEnum.OnWaveStarted.Invoke(new GameEventInfo());
    }

    private void Update()
    {
        _waveTimer += Time.deltaTime;

        foreach (EnemySpawner enemySpawner in _currentWaveInfo)
        {
            if (!enemySpawner.ShouldSpawn(_waveTimer))
            {
                continue;
            }
            
            //enemySpawner.Infos.Enemy todo: instantiate enemy
        }
    }
}
