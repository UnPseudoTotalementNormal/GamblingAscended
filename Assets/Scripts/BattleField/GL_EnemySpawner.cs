using Character.Enemy;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyCharacter;
    [SerializeField] private GL_PathTracer _pathTracer;
    
    private GameEventEnum _spawnEnemyEvent = GameEventEnum.SpawnEnemy;

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
    }
}
