using UnityEngine;

public class UIWaveProgress : MonoBehaviour
{
    public WaveManager waveManager;
    public RectTransform waveBarFill;
    public RectTransform waveBarBackground;
    public float animationSpeed = 8f;

    private float maxWidth;

    void Start()
    {
        if (waveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
        }

        maxWidth = waveBarBackground.rect.width;
        SetFillWidth(maxWidth);
    }

    void Update()
    {
        if (waveManager == null || waveBarFill == null || waveBarBackground == null)
        {
            return;
        }

        float targetPercent = waveManager.GetProgressPercent();
        float targetWidth = Mathf.Clamp01(targetPercent) * maxWidth;
        float currentWidth = waveBarFill.rect.width;
        float newWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * animationSpeed);

        SetFillWidth(newWidth);
    }

    void SetFillWidth(float width)
    {
        waveBarFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(width, 0f, maxWidth));
    }
}
