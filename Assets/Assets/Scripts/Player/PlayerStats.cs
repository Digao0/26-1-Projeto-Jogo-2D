using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    public float maxHealth = 100f;

    public void AddDamage(float value)
    {
        damage += value;
        Debug.Log("Damage +" + value);
    }

    public void AddSpeed(float value)
    {
        speed += value;
        Debug.Log("Speed +" + value);
    }

    public void AddHealth(float value)
    {
        maxHealth += value;
        Debug.Log("Health +" + value);
    }
}