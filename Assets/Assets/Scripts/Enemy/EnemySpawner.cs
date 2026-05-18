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

    // ===== CASTELO =====
    public GameObject armoredAxemanPrefab;
    public GameObject lancerPrefab;
    public GameObject knightTemplarPrefab;
    public GameObject werewolfPrefab;
    public GameObject soldierPrefab;

    // ===== CAVERNA 🔥 =====
    public GameObject armoredSkeletonPrefab;
    public GameObject werebearPrefab;
    public GameObject greatswordSkeletonPrefab;
    public GameObject skeletonArcherPrefab;
    public GameObject SkeletonPrefab;

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

            // CASTELO
            case "ArmoredAxeman": prefab = armoredAxemanPrefab; break;
            case "Lancer": prefab = lancerPrefab; break;
            case "KnightTemplar": prefab = knightTemplarPrefab; break;
            case "Werewolf": prefab = werewolfPrefab; break;
            case "Soldier": prefab = soldierPrefab; break;

            // CAVERNA 🔥
            case "Skeleton": prefab = SkeletonPrefab; break;
            case "ArmoredSkeleton": prefab = armoredSkeletonPrefab; break;
            case "Werebear": prefab = werebearPrefab; break;
            case "GreatswordSkeleton": prefab = greatswordSkeletonPrefab; break;
            case "SkeletonArcher": prefab = skeletonArcherPrefab; break;
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
        Vector2 pos;
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: pos = new Vector2(Random.Range(-spawnRangeX, spawnRangeX),  spawnRangeY); break; // topo
            case 1: pos = new Vector2(Random.Range(-spawnRangeX, spawnRangeX), -spawnRangeY); break; // baixo
            case 2: pos = new Vector2(-spawnRangeX, Random.Range(-spawnRangeY, spawnRangeY)); break; // esquerda
            default: pos = new Vector2( spawnRangeX, Random.Range(-spawnRangeY, spawnRangeY)); break; // direita
        }
        Instantiate(prefab, pos, Quaternion.identity);
    }
}