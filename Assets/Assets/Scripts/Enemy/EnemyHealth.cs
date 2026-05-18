using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData data;

    public int maxHealth = 50;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.3f;
    public float deathAnimDuration = 1f;
    public GameObject dropPrefab;

    private int currentHealth;
    private EnemyFollow enemyFollow;
    private Rigidbody2D rb;
    private EnemyAnimator enemyAnimator;
    private Coroutine burnCoroutine;

    void Start()
    {
        if (data != null)
        {
            maxHealth = data.maxHealth;
            knockbackForce = data.knockbackForce;
            knockbackDuration = data.knockbackDuration;
            deathAnimDuration = data.deathAnimDuration;
        }

        currentHealth = maxHealth;
        enemyFollow = GetComponent<EnemyFollow>();
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        DamageNumber.Spawn(transform.position, damage, Color.white);
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

    public void ApplyBurn(int dps, float duration)
    {
        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(BurnRoutine(dps, duration));
    }

    IEnumerator BurnRoutine(int dps, float duration)
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        Color[] originalColors = System.Array.ConvertAll(srs, s => s.color);
        foreach (var s in srs) s.color = new Color(1f, 0.45f, 0.1f);

        float elapsed = 0f;
        while (elapsed < duration && currentHealth > 0)
        {
            yield return new WaitForSeconds(0.5f);
            elapsed += 0.5f;
            if (currentHealth <= 0) break;
            DamageNumber.Spawn(transform.position, dps, new Color(1f, 0.4f, 0f));
            currentHealth -= dps;
            if (currentHealth <= 0) { Die(); yield break; }
        }

        for (int i = 0; i < srs.Length; i++)
            if (srs[i] != null) srs[i].color = originalColors[i];
        burnCoroutine = null;
    }

    void Die()
    {
        enemyFollow.enabled = false;
        enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        if (enemyAnimator != null)
        {
            enemyAnimator.PlayDeath(deathAnimDuration);
        }
        else
        {
            Destroy(gameObject);
        }

        if (dropPrefab != null)
            Instantiate(dropPrefab, transform.position, Quaternion.identity);

        FindObjectOfType<WaveManager>()?.EnemyDied();
    }
}
