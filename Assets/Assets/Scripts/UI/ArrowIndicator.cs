using UnityEngine;
using UnityEngine.UI;

public class ArrowIndicator : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public WaveManager waveManager;

    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (!waveManager.isFinished)
        {
            img.enabled = false; // 🔥 só esconde
            return;
        }

        img.enabled = true; // 🔥 mostra de novo

        Vector3 direction = target.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}