using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    public string sceneName = "CastleScene";
    public WaveManager waveManager;
    public bool showItemSelection = false;

    private Collider2D col;
    private bool triggered = false;

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
        if (!collision.CompareTag("Player") || triggered) return;
        triggered = true;

        if (showItemSelection)
            ItemSelectionUI.Show(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}
