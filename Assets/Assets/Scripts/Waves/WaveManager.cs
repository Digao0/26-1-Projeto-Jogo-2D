using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int waveNumber = 1;
    public int enemiesPerWave = 3;

    public float spawnRangeX = 10f;
    public float spawnRangeY = 5f;

    private int enemiesAlive = 0;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        // se não tem inimigos vivos → próxima wave
        if (enemiesAlive <= 0)
        {
            waveNumber++;
            enemiesPerWave += 2;

            StartWave();
        }
    }

    void StartWave()
    {
        enemiesAlive = enemiesPerWave;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(-spawnRangeX, spawnRangeX),
                Random.Range(-spawnRangeY, spawnRangeY)
            );

            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }

        Debug.Log("Wave " + waveNumber);
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }
}