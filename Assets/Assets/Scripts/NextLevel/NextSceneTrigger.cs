using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    public string sceneName = "CastleScene";
    public WaveManager waveManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // só funciona quando terminar as waves
        if (waveManager != null && !waveManager.isFinished) return;

        SceneManager.LoadScene(sceneName);
    }
}