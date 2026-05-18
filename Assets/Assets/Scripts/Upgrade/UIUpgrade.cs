using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum UpgradeType
{
    Damage,
    Speed,
    Health
}



public class UIUpgrade : MonoBehaviour
{
    public PlayerStats playerStats;
    public WaveManager waveManager;

    public Button option1;
    public Button option2;
    public Button option3;

    Rarity GetRandomRarity()
    {
        float roll = Random.value;

        if (roll < 0.5f)
            return Rarity.Common;
        else if (roll < 0.8f)
            return Rarity.Rare;
        else if (roll < 0.95f)
            return Rarity.Epic;
        else
            return Rarity.Legendary;
    }

    float GetValue(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 5f;
            case Rarity.Rare: return 10f;
            case Rarity.Epic: return 20f;
            case Rarity.Legendary: return 40f;
        }
        return 5f;
    }

    Color GetColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.gray;
            case Rarity.Rare: return Color.blue;
            case Rarity.Epic: return new Color(0.6f, 0, 0.8f);
            case Rarity.Legendary: return Color.yellow;
        }
        return Color.white;
    }
    void Start()
    {
        Hide();
    }

    public void Show()
    {
        Debug.Log("Abrindo painel de upgrade");

        Time.timeScale = 0f;
        gameObject.SetActive(true);

        Setup(option1);
        Setup(option2);
        Setup(option3);
    }

    void Setup(Button btn)
    {
        TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();

        Rarity rarity = GetRandomRarity();
        float value = GetValue(rarity);
        UpgradeType type = GetRandomType();

        string statName = "";

        if (type == UpgradeType.Damage) statName = "Dano";
        else if (type == UpgradeType.Speed) statName = "Velocidade";
        else statName = "Vida";

        if (txt != null)
            txt.text = "+ " + statName + " (" + rarity.ToString() + ")";

        Image img = btn.GetComponent<Image>();
        if (img != null)
            img.color = GetColor(rarity);

        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(() =>
        {
            ApplyUpgrade(type, value);
        });
    }
    void ApplyUpgrade(UpgradeType type, float value)
    {
        if (type == UpgradeType.Damage)
            playerStats.AddDamage(value);
        else if (type == UpgradeType.Speed)
            playerStats.AddSpeed(value);
        else
            playerStats.AddHealth(value);

        Close();
    }
    void Close()
    {
        Debug.Log("Fechando painel");

        gameObject.SetActive(false);
        Time.timeScale = 1f;

        waveManager.StartWave();
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
    
    UpgradeType GetRandomType()
    {
        int roll = Random.Range(0, 3);

        if (roll == 0) return UpgradeType.Damage;
        if (roll == 1) return UpgradeType.Speed;
        return UpgradeType.Health;
    }
    

}