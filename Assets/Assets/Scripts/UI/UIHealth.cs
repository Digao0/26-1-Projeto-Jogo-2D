using TMPro;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    public PlayerHealth player;
    public TextMeshProUGUI healthText;

    void Update()
    {
        healthText.text = "HP: " + player.GetCurrentHealth();
    }
}