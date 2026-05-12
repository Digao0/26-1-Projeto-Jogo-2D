using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private SpriteRenderer sr;
    PlayerHealth health;
    PlayerAttack playerAttack;

    private float speedMultiplier = 1f;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        movement.x = 0;
        movement.y = 0;

        if (Input.GetKey(KeyCode.A)) movement.x = -1;
        if (Input.GetKey(KeyCode.D)) movement.x = 1;
        if (Input.GetKey(KeyCode.W)) movement.y = 1;
        if (Input.GetKey(KeyCode.S)) movement.y = -1;

        anim.SetFloat("Speed", movement.magnitude);

        if (!playerAttack.isAttacking)
        {
            if (movement.x > 0)
                sr.flipX = false;
            else if (movement.x < 0)
                sr.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (health.isKnockedBack) return;
        rb.linearVelocity = movement.normalized * speed * speedMultiplier;
    }

    public void AplicarLentidao(float fator, float duracao)
    {
        speedMultiplier = fator;
        CancelInvoke(nameof(ResetVelocidade));
        Invoke(nameof(ResetVelocidade), duracao);
    }

    void ResetVelocidade()
    {
        speedMultiplier = 1f;
    }
}