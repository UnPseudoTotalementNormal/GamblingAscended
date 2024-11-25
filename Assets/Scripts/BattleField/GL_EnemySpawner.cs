using System.Collections.Generic;
using Character.Enemy;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyCharacter;
    [SerializeField] private GL_PathTracer _pathTracer;
    
    private GameEventEnum _spawnEnemyEvent = GameEventEnum.SpawnEnemy;

    private List<GameObject> _aliveEnemies;

    private void Start()
    {
        _spawnEnemyEvent.AddListener(OnSpawnEnemyEvent);
    }

    private void OnSpawnEnemyEvent(GameEventInfo eventInfo)
    {
        if (!eventInfo.TryTo(out GameEventSpawnEnemy spawnEnemyInfo))
        {
            return;
        }
        
        SpawnEnemy(spawnEnemyInfo.EnemyObject);
    }

    private void SpawnEnemy(GL_EnemyObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy.Prefab, _pathTracer.Waypoints[0], Quaternion.identity);
        var pathFollower = newEnemy.GetComponent<GL_PathFollower>();
        pathFollower.Init(_pathTracer.Waypoints);
        
        _aliveEnemies.Add(newEnemy);
    }

    public List<GameObject> GetAliveEnemies()
    {
        for (var i = 0; i < _aliveEnemies.Count; i++)
        {
            var enemy = _aliveEnemies[i];
            if (enemy) continue;
            _aliveEnemies.RemoveAt(i);
            i--;
        }

        return _aliveEnemies;
    }
}
