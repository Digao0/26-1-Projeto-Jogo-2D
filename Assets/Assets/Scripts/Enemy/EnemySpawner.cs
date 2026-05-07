using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;

    public float spawnRangeX = 10f;
    public float spawnRangeY = 5f;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnRangeX, spawnRangeX),
                Random.Range(-spawnRangeY, spawnRangeY)
            );

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}