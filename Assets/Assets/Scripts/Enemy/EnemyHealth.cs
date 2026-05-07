using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.3f;
    public float deathAnimDuration = 1f;

    private int currentHealth;
    private EnemyFollow enemyFollow;
    private Rigidbody2D rb;
    private EnemyAnimator enemyAnimator;

    void Start()
    {
        currentHealth = maxHealth;
        enemyFollow = GetComponent<EnemyFollow>();
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        enemyAnimator?.PlayHurt();
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
        if (enemyAnimator != null)
        {
            enemyFollow.enabled = false;
            enabled = false;
            enemyAnimator.PlayDeath(deathAnimDuration);
        }
        else
        {
            Destroy(gameObject);
        }
        FindObjectOfType<WaveManager>().EnemyDied();
        Destroy(gameObject);
    }
}
