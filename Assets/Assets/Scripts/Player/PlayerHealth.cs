using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float knockbackForce = 5f;
    public float invulnerabilityTime = 0.5f;
    public float knockbackDuration = 0.2f;

    public bool isKnockedBack = false;

    private int currentHealth;

    private Animator anim;
    private Rigidbody2D rb;
    private PlayerStats stats;

    private bool isInvulnerable = false;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        currentHealth = (int)stats.maxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform enemy)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        isKnockedBack = true;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        anim.SetTrigger("Hit");

        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * knockbackForce;

        CancelInvoke(nameof(StopKnockback));
        Invoke(nameof(StopKnockback), knockbackDuration);

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

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        anim.SetTrigger("Die");

        Time.timeScale = 0f;

        StartCoroutine(GameOverDelay());
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        CallGameOver();
    }

    void CallGameOver()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.GameOver();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, (int)stats.maxHealth);
    }
    
    public void TakeDamageNoKnockback(int damage)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        anim.SetTrigger("Hit");

        isInvulnerable = true;
        Invoke(nameof(ResetInvulnerability), invulnerabilityTime);

        if (currentHealth <= 0)
            Die();
    }
}