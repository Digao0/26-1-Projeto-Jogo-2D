using System.Collections;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    public float attackCooldown = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private EnemyAnimator enemyAnimator;
    private bool isAttacking;
    private float nextAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
    }

    void OnEnable()
    {
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (isAttacking || Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;
        StartCoroutine(AttackRoutine(collision.gameObject));
    }

    IEnumerator AttackRoutine(GameObject playerGo)
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        enemyAnimator?.PlayAttack();

        yield return new WaitForSeconds(0.25f);
        playerGo?.GetComponent<PlayerHealth>()?.TakeDamage(damage, transform);

        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }
}
