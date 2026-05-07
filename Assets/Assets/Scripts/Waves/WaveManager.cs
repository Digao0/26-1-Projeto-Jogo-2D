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
    private bool isChangingWave;

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

        enemiesAlive = enemiesPerWave;
        enemySpawner.SpawnEnemies(enemiesPerWave);

        Debug.Log("Wave " + waveNumber + " - Inimigos: " + enemiesPerWave);
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

        yield return new WaitForSeconds(timeBetweenWaves);

        waveNumber++;
        enemiesPerWave += enemiesIncrement;

        StartWave();
        isChangingWave = false;
    }
}
