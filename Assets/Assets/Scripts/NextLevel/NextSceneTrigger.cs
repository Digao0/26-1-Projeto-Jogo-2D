using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    public string sceneName = "CastleScene";
    public WaveManager waveManager;

    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = false;
    }

    void Update()
    {
        if (waveManager == null) return;
        if (waveManager.isFinished && !col.isTrigger)
            col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        SceneManager.LoadScene(sceneName);
    }
}
