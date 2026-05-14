using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // ===== FLORESTA =====
    public GameObject enemyPrefab;
    public GameObject slimePrefab;
    public GameObject orcPrefab;
    public GameObject riderPrefab;
    public GameObject armoredPrefab;
    public GameObject elitePrefab;

    // ===== EXTRA =====
    public GameObject soldierPrefab;
    public GameObject skeletonPrefab;

    // ===== CASTELO =====
    public GameObject armoredAxemanPrefab;
    public GameObject lancerPrefab;
    public GameObject knightTemplarPrefab;
    public GameObject werewolfPrefab;

    public float spawnRangeX = 10f;
    public float spawnRangeY = 5f;

    // =============================

    public void SpawnEnemies(int amount)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner precisa de um enemyPrefab configurado.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            Spawn(enemyPrefab);
        }
    }

    // =============================

    public void SpawnSpecific(string type)
    {
        GameObject prefab = null;

        switch (type)
        {
            // FLORESTA
            case "Slime": prefab = slimePrefab; break;
            case "Orc": prefab = orcPrefab; break;
            case "Rider": prefab = riderPrefab; break;
            case "Armored": prefab = armoredPrefab; break;
            case "Elite": prefab = elitePrefab; break;

            // EXTRA
            case "Soldier": prefab = soldierPrefab; break;
            case "Skeleton": prefab = skeletonPrefab; break;

            // CASTELO 🔥
            case "ArmoredAxeman": prefab = armoredAxemanPrefab; break;
            case "Lancer": prefab = lancerPrefab; break;
            case "KnightTemplar": prefab = knightTemplarPrefab; break;
            case "Werewolf": prefab = werewolfPrefab; break;
        }

        if (prefab == null)
        {
            Debug.LogWarning("Prefab não encontrado para tipo: " + type);
            return;
        }

        Spawn(prefab);
    }

    // =============================

    void Spawn(GameObject prefab)
    {
        Vector2 pos = new Vector2(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(-spawnRangeY, spawnRangeY)
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }
}