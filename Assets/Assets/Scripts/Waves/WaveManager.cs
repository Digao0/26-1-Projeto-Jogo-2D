using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    // FLORESTA
    public int slime;
    public int orc;
    public int rider;
    public int armored;
    public int elite;

    // CASTELO
    public int armoredAxeman;
    public int lancer;
    public int knightTemplar;
    public int werewolf;
    public int soldier;

    // CAVERNA 🔥
    public int skeleton;
    public int armoredSkeleton;
    public int werebear;
    public int greatswordSkeleton;
    public int skeletonArcher;
}

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Wave[] waves;

    public int waveNumber = 1;
    public float timeBetweenWaves = 2f;

    private int enemiesAlive;
    private int enemiesThisWave;
    private bool isChangingWave;
    private float nextWaveProgress = 1f;

    public bool isFinished = false;

    void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }

        StartWave();
    }

    void StartWave()
    {
        if (waveNumber - 1 >= waves.Length)
        {
            Debug.Log("Fim da fase");
            isFinished = true;
            return;
        }

        Wave currentWave = waves[waveNumber - 1];

        enemiesAlive = 0;

        // FLORESTA
        SpawnAndCount("Slime", currentWave.slime);
        SpawnAndCount("Orc", currentWave.orc);
        SpawnAndCount("Rider", currentWave.rider);
        SpawnAndCount("Armored", currentWave.armored);
        SpawnAndCount("Elite", currentWave.elite);

        // CASTELO
        SpawnAndCount("ArmoredAxeman", currentWave.armoredAxeman);
        SpawnAndCount("Lancer", currentWave.lancer);
        SpawnAndCount("KnightTemplar", currentWave.knightTemplar);
        SpawnAndCount("Werewolf", currentWave.werewolf);
        SpawnAndCount("Soldier", currentWave.soldier);

        // CAVERNA 🔥
        SpawnAndCount("Skeleton", currentWave.skeleton);
        SpawnAndCount("ArmoredSkeleton", currentWave.armoredSkeleton);
        SpawnAndCount("Werebear", currentWave.werebear);
        SpawnAndCount("GreatswordSkeleton", currentWave.greatswordSkeleton);
        SpawnAndCount("SkeletonArcher", currentWave.skeletonArcher);

        enemiesThisWave = enemiesAlive;
        nextWaveProgress = 1f;

        Debug.Log("Wave " + waveNumber + " - Inimigos: " + enemiesThisWave);
    }

    void SpawnAndCount(string type, int amount)
    {
        if (amount <= 0) return;

        for (int i = 0; i < amount; i++)
        {
            enemySpawner.SpawnSpecific(type);
            enemiesAlive++;
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !isChangingWave && !isFinished)
        {
            StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        isChangingWave = true;
        nextWaveProgress = 0f;

        float timer = 0f;

        while (timer < timeBetweenWaves)
        {
            timer += Time.deltaTime;
            nextWaveProgress = Mathf.Clamp01(timer / timeBetweenWaves);
            yield return null;
        }

        waveNumber++;
        StartWave();
        isChangingWave = false;
    }

    public float GetProgressPercent()
    {
        if (isChangingWave)
            return nextWaveProgress;

        if (enemiesThisWave <= 0)
            return 0f;

        return Mathf.Clamp01((float)enemiesAlive / enemiesThisWave);
    }

    public bool IsChangingWave()
    {
        return isChangingWave;
    }

    public int GetTotalWaves()
    {
        return waves.Length;
    }
}