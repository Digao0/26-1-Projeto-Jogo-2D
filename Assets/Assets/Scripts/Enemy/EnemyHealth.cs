using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.3f;

    private int currentHealth;
    private EnemyFollow enemyFollow;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        enemyFollow = GetComponent<EnemyFollow>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        ApplyKnockback(attacker);
    }

    void ApplyKnockback(Transform attacker)
    {
        enemyFollow.enabled = false;
        Vector2 direction = (transform.position - attacker.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        Invoke(nameof(StopKnockback), knockbackDuration);
    }

    void StopKnockback()
    {
        rb.linearVelocity = Vector2.zero;
        enemyFollow.enabled = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
