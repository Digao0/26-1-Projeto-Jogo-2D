using UnityEngine;
using UnityEngine.UI;
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
        ApplySwordBarColor();
    }

    void ApplySwordBarColor()
    {
        if (PlayerSwordManager.Instance == null) return;
        Image fill = healthBarFill.GetComponent<Image>();
        if (fill == null) return;

        fill.color = PlayerSwordManager.Instance.equippedSword switch
        {
            SwordType.Fire => new Color(0.90f, 0.15f, 0.05f),
            SwordType.Ice  => new Color(0.15f, 0.60f, 1.00f),
            SwordType.Life => new Color(0.15f, 0.85f, 0.25f),
            _              => fill.color
        };
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