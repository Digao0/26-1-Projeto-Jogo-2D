using System.Collections;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public EnemyData data;

    public float speed = 2f;
    public int damage = 10;
    public float attackCooldown = 1f;

    [HideInInspector] public LayerMask obstacleMask;

    private Transform player;
    private Rigidbody2D rb;
    private EnemyAnimator enemyAnimator;
    private SpriteRenderer sr;

    private bool isAttacking;
    private float nextAttackTime;

    // Detecção de travamento por posição real
    private Vector2 lastCheckedPosition;
    private float positionCheckTimer;
    private float stuckTimer;

    // Direção de fuga quando travado
    private Vector2 wanderDir;
    private float wanderTimer;

    private readonly ContactPoint2D[] contacts = new ContactPoint2D[8];
    private const float RayDistance = 1.2f;
    private const float RayRadius   = 0.25f;

    void Start()
    {
        if (data != null)
        {
            speed         = data.speed;
            damage        = data.damage;
            attackCooldown = data.attackCooldown;
        }

        player        = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb            = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        sr            = GetComponentInChildren<SpriteRenderer>();
        lastCheckedPosition = rb.position;
    }

    void OnEnable()
    {
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking) return;

        Vector2 moveDir;

        if (wanderTimer > 0f)
        {
            // Modo fuga: anda na direção de escape por um tempo fixo
            wanderTimer -= Time.fixedDeltaTime;
            moveDir = wanderDir;
        }
        else
        {
            Vector2 toPlayer = ((Vector2)player.position - rb.position).normalized;
            moveDir = GetSteeringDirection(toPlayer);
            moveDir = SlideAlongWalls(moveDir);
        }

        rb.linearVelocity = moveDir * speed;

        if (moveDir.x > 0) sr.flipX = false;
        else if (moveDir.x < 0) sr.flipX = true;

        CheckStuck();
    }

    // Testa a direção direta e ângulos alternativos usando CircleCast
    // filtrando por tag em vez de layer — não precisa configurar nada
    Vector2 GetSteeringDirection(Vector2 desired)
    {
        if (!HasObstacle(desired))
            return desired;

        for (int angle = 45; angle <= 135; angle += 45)
        {
            Vector2 left  = Rotate(desired,  angle);
            Vector2 right = Rotate(desired, -angle);

            if (!HasObstacle(left))  return left;
            if (!HasObstacle(right)) return right;
        }

        return desired;
    }

    bool HasObstacle(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(rb.position, RayRadius, direction, RayDistance);
        foreach (var hit in hits)
        {
            if (hit.collider == null)                         continue;
            if (hit.collider.gameObject == gameObject)        continue; // ignora si mesmo
            if (hit.collider.isTrigger)                       continue; // ignora triggers
            if (hit.collider.CompareTag("Player"))            continue;
            if (hit.collider.CompareTag("Enemy"))             continue;
            return true; // achou obstáculo real
        }
        return false;
    }

    // Desliza ao longo de paredes usando os contatos físicos reais
    Vector2 SlideAlongWalls(Vector2 desired)
    {
        int count = rb.GetContacts(contacts);
        Vector2 result = desired;

        for (int i = 0; i < count; i++)
        {
            Collider2D other = contacts[i].collider;
            if (other.CompareTag("Player") || other.CompareTag("Enemy")) continue;

            Vector2 normal = contacts[i].normal;
            float dot = Vector2.Dot(result, -normal);
            if (dot > 0f)
                result += normal * dot;
        }

        return result.sqrMagnitude > 0.01f ? result.normalized : desired;
    }

    // Detecta travamento por deslocamento real a cada 0.5s
    // Se travado por 1.5s, entra em modo fuga perpendicular por 1-2s
    void CheckStuck()
    {
        positionCheckTimer += Time.fixedDeltaTime;
        if (positionCheckTimer < 0.5f) return;

        float moved = Vector2.Distance(rb.position, lastCheckedPosition);
        lastCheckedPosition = rb.position;
        positionCheckTimer  = 0f;

        if (moved < 0.05f && wanderTimer <= 0f)
        {
            stuckTimer += 0.5f;
            if (stuckTimer >= 1.5f)
            {
                Vector2 toPlayer = ((Vector2)player.position - rb.position).normalized;
                Vector2 perp = new Vector2(-toPlayer.y, toPlayer.x);
                if (Random.value > 0.5f) perp = -perp;

                wanderDir   = perp;
                wanderTimer = Random.Range(1f, 2f);
                stuckTimer  = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (isAttacking || Time.time < nextAttackTime)  return;

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
