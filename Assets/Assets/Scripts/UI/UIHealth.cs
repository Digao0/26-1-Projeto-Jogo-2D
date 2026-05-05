using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public PlayerHealth player;
    public RectTransform healthBarFill;
    public RectTransform healthBarBackground;

    private float maxWidth;

    void Start()
    {
        maxWidth = healthBarBackground.rect.width;
    }

    void Update()
    {
        float targetPercent = (float)player.GetCurrentHealth() / player.maxHealth;
        float currentWidth = healthBarFill.sizeDelta.x;
        float newWidth = Mathf.Lerp(currentWidth, targetPercent * maxWidth, Time.deltaTime * 8f);
        healthBarFill.sizeDelta = new Vector2(newWidth, healthBarFill.sizeDelta.y);
    }
}
