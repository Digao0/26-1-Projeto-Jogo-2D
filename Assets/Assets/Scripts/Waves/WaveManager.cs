using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    public int waveNumber = 1;
    public int enemiesPerWave = 3;
    public int enemiesIncrement = 2;
    public float timeBetweenWaves = 2f;

    private int enemiesAlive;
    private int enemiesThisWave;
    private bool isChangingWave;
    private float nextWaveProgress = 1f;

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

        enemiesThisWave = enemiesPerWave;
        enemiesAlive = enemiesThisWave;
        nextWaveProgress = 1f;

        enemySpawner.SpawnEnemies(enemiesThisWave);

        Debug.Log("Wave " + waveNumber + " - Inimigos: " + enemiesThisWave);
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
        enemiesPerWave += enemiesIncrement;

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
