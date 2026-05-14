using TMPro;
using UnityEngine;

public class UIWave : MonoBehaviour
{
    public WaveManager waveManager;
    public TextMeshProUGUI waveText;
    public GameObject progressBar;

    void Update()
    {
        if (waveManager.isFinished)
        {
            waveText.text = "Prossiga para a próxima fase";

            if (progressBar != null)
                progressBar.SetActive(false);

            return;
        }

        if (progressBar != null)
            progressBar.SetActive(true);

        int current = waveManager.waveNumber;
        int total = waveManager.GetTotalWaves();

        waveText.text = "Wave " + current + " / " + total;
    }
}