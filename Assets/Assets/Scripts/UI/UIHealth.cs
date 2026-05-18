using UnityEngine;
using TMPro;

public class UIHealth : MonoBehaviour
{
    public PlayerHealth player;
    public RectTransform healthBarFill;
    public RectTransform healthBarBackground;
    public TextMeshProUGUI healthText;

    private float maxWidth;
    private PlayerStats stats;

    void Start()
    {
        maxWidth = healthBarBackground.rect.width;
        stats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player == null || stats == null) return;

        float currentHealth = player.GetCurrentHealth();
        float maxHealth = stats.maxHealth;

        float targetPercent = currentHealth / maxHealth;

        float currentWidth = healthBarFill.sizeDelta.x;
        float newWidth = Mathf.Lerp(currentWidth, targetPercent * maxWidth, Time.unscaledDeltaTime * 8f);

        healthBarFill.sizeDelta = new Vector2(newWidth, healthBarFill.sizeDelta.y);

        if (healthText != null)
            healthText.text = (int)currentHealth + " / " + (int)maxHealth;
    }
}