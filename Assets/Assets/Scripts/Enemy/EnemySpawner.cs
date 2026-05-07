using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRangeX = 10f;
    public float spawnRangeY = 5f;

    public void SpawnEnemies(int amount)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner precisa de um enemyPrefab configurado.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnRangeX, spawnRangeX),
                Random.Range(-spawnRangeY, spawnRangeY)
            );

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
