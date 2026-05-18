using System.Collections;
using UnityEngine;

public class FogCenterController : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 2f;

    private WaveManager waveManager;
    private SpriteRenderer spriteRenderer;
    private bool isFading = false;

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (waveManager != null && waveManager.isFinished && !isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;

        if (spriteRenderer != null)
        {
            Color startColor = spriteRenderer.color;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(startColor.a, 0f, timer / fadeDuration);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(fadeDuration);
        }

        gameObject.SetActive(false);
    }
}
