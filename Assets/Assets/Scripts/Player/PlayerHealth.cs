using UnityEngine;
using System.Collections;

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
        if (isInvulnerable || currentHealth <= 0) return;
        isKnockedBack = true;

        currentHealth -= damage;

        GetComponent<PlayerAttack>().ResetAttack();

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

    public void GrantInvulnerability(float duration)
    {
        isInvulnerable = true;
        CancelInvoke(nameof(ResetInvulnerability));
        Invoke(nameof(ResetInvulnerability), duration);
    }

    void ResetInvulnerability()
    {
        isInvulnerable = false;
    }

    void Die()
    {
        Debug.Log("Player morreu");

        // trava movimento
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // animação de morte
        anim.SetTrigger("Die");

        // 🔥 pausa o jogo imediatamente
        Time.timeScale = 0f;

        // espera animação terminar usando tempo real
        StartCoroutine(GameOverDelay());
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    void CallGameOver()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.GameOver();
    }
    
    IEnumerator GameOverDelay()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        CallGameOver();
    }
    public void TakeDamageNoKnockback(int damage)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        currentHealth -= damage;
        anim.SetTrigger("Hit");

        isInvulnerable = true;
        Invoke(nameof(ResetInvulnerability), invulnerabilityTime);

        if (currentHealth <= 0)
            Die();
    }
    
}