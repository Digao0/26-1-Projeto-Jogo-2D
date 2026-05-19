using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    public float maxHealth = 100f;

    private static float bonusDamage = 0f;
    private static float bonusSpeed = 0f;
    private static float bonusHealth = 0f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics() { bonusDamage = 0f; bonusSpeed = 0f; bonusHealth = 0f; }

    void Awake()
    {
        damage += bonusDamage;
        speed += bonusSpeed;
        maxHealth += bonusHealth;
    }

    public void AddDamage(float value)
    {
        damage += value;
        bonusDamage += value;
    }

    public void AddSpeed(float value)
    {
        speed += value;
        bonusSpeed += value;
    }

    public void AddHealth(float value)
    {
        maxHealth += value;
        bonusHealth += value;
    }

    public static void ResetBonuses()
    {
        bonusDamage = 0f;
        bonusSpeed = 0f;
        bonusHealth = 0f;
    }
}