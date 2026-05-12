using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int slime;
    public int orc;
    public int rider;
    public int armored;
    public int elite;
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
        if (enemySpawner == null)
        {
            Debug.LogError("WaveManager precisa de um EnemySpawner na cena.");
            return;
        }

        if (waveNumber - 1 >= waves.Length)
        {
            Debug.Log("Fim da fase");
            isFinished = true;
            return;
        }

        Wave currentWave = waves[waveNumber - 1];

        enemiesAlive = 0;

        // Spawn por tipo
        SpawnAndCount("Slime", currentWave.slime);
        SpawnAndCount("Orc", currentWave.orc);
        SpawnAndCount("Rider", currentWave.rider);
        SpawnAndCount("Armored", currentWave.armored);
        SpawnAndCount("Elite", currentWave.elite);

        enemiesThisWave = enemiesAlive;

        nextWaveProgress = 1f;

        Debug.Log("Wave " + waveNumber + " - Inimigos: " + enemiesThisWave);
    }

    void SpawnAndCount(string type, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            enemySpawner.SpawnSpecific(type);
            enemiesAlive++;
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !isChangingWave)
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
        {
            return nextWaveProgress;
        }

        if (enemiesThisWave <= 0)
        {
            return 0f;
        }

        return Mathf.Clamp01((float)enemiesAlive / enemiesThisWave);
    }

    public bool IsChangingWave()
    {
        return isChangingWave;
    }
}