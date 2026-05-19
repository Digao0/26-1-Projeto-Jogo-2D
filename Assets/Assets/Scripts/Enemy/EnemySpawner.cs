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

    [Header("Spawn ao redor do jogador")]
    public float spawnMinDistance = 6f;
    public float spawnMaxDistance = 10f;

    [Header("Pontos fixos de spawn (opcional - ignora distância se preenchido)")]
    public Transform[] spawnPoints;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

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

    public bool SpawnSpecific(string type)
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
            return false;
        }

        Spawn(prefab);
        return true;
    }

    // =============================

    void Spawn(GameObject prefab)
    {
        Vector2 pos;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            pos = point.position;
        }
        else
        {
            Vector2 center = player != null ? (Vector2)player.position : Vector2.zero;
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(spawnMinDistance, spawnMaxDistance);
            pos = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        }

        Instantiate(prefab, pos, Quaternion.identity);
    }
}