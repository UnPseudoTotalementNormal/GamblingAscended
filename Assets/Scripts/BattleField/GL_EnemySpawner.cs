using Character.Enemy;
using UnityEngine;

public class GL_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyCharacter;
    [SerializeField] private GL_PathTracer _pathTracer;

    private void Start()
    {
        SpawnEnemy(_enemyCharacter);
    }

    private void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, _pathTracer.Waypoints[0], Quaternion.identity);
        var pathFollower = newEnemy.GetComponent<GL_PathFollower>();
        pathFollower.Init(_pathTracer.Waypoints);
    }
}
