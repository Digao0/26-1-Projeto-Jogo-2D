using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject slimePrefab;
    public GameObject orcPrefab;
    public GameObject riderPrefab;
    public GameObject armoredPrefab;
    public GameObject elitePrefab;

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
    
    public void SpawnSpecific(string type)
    {
        GameObject prefab = null;

        switch (type)
        {
            case "Slime": prefab = slimePrefab; break;
            case "Orc": prefab = orcPrefab; break;
            case "Rider": prefab = riderPrefab; break;
            case "Armored": prefab = armoredPrefab; break;
            case "Elite": prefab = elitePrefab; break;
        }

        if (prefab == null) return;

        Vector2 pos = new Vector2(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(-spawnRangeY, spawnRangeY)
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }
}
