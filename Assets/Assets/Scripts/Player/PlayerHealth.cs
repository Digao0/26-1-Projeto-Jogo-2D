using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float knockbackForce = 5f;
    public float invulnerabilityTime = 0.5f;
    public float knockbackDuration = 0.2f;
    public bool isKnockedBack = false;

    private int currentHealth;
    private Animator anim;
    private Rigidbody2D rb;

    private bool isInvulnerable = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform enemy)
    {
        if (isInvulnerable) return;
        isKnockedBack = true;

        currentHealth -= damage;

        // animação de dano
        anim.SetTrigger("Hit");

        // direção do knockback
        Vector2 direction = (transform.position - enemy.position).normalized;

        // aplica knockback
        rb.linearVelocity = direction * knockbackForce;

        // para o knockback depois de um tempo
        CancelInvoke(nameof(StopKnockback));
        Invoke(nameof(StopKnockback), knockbackDuration);

        // ativa invulnerabilidade
        isInvulnerable = true;
        Invoke(nameof(ResetInvulnerability), invulnerabilityTime);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void StopKnockback()
    {
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    void ResetInvulnerability()
    {
        isInvulnerable = false;
    }

    void Die()
    {
        Debug.Log("Player morreu");
        // aqui você coloca game over depois
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    
}