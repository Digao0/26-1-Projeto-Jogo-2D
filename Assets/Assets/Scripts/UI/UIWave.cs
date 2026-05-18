using TMPro;
using UnityEngine;

public class UIWave : MonoBehaviour
{
    public WaveManager waveManager;
    public TextMeshProUGUI waveText;
    public GameObject progressBar;

    void Start()
    {
        var font = Resources.Load<TMP_FontAsset>("Fonts/MedievalSharp-Regular SDF");
        if (font != null) waveText.font = font;
        UpdateText();
    }

    void Update()
    {
        UpdateText();
    }

    void UpdateText()
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