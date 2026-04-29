using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 25;
    public float attackCooldown = 0.4f;
    public float attackRange = 1.8f;
    public Vector2 attackSize = new Vector2(1.5f, 1.2f);
    public float attackIframes = 0.3f;

    private Animator anim;
    private SpriteRenderer sr;
    private PlayerHealth playerHealth;
    private float cooldownTimer;
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0 || v != 0)
            lastDirection = new Vector2(h, v).normalized;

        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0)
            Attack();
    }

    void Attack()
    {
        cooldownTimer = attackCooldown;
        anim.SetTrigger("Attack");
        playerHealth.GrantInvulnerability(attackIframes);

        Vector2 hitCenter = (Vector2)transform.position + lastDirection * attackRange;
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitCenter, attackSize, 0f);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                hit.GetComponent<EnemyHealth>()?.TakeDamage(damage, transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 hitCenter = (Vector2)transform.position + lastDirection * attackRange;
        Gizmos.DrawWireCube(hitCenter, attackSize);
    }
}
