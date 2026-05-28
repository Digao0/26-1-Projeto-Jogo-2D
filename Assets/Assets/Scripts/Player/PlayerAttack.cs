using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackUp;
    public GameObject attackDown;
    public GameObject attackLeft;
    public GameObject attackRight;

    private Animator anim;
    private SpriteRenderer sr;
    private PlayerStats stats;

    private bool canAttack = true;
    public bool isAttacking = false;

    private string lastDirection = "Down";
    private Vector2 _lastMobileDir = Vector2.down;

    [Header("Power-up de Dano")]
    public float damageMultiplier = 1f;
    public AudioClip boostMusic;
    private Coroutine boostCoroutine;

    void Start()
    {
        anim  = GetComponent<Animator>();
        sr    = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Atualiza direção a partir do joystick (mesmo sem atacar)
        if (MobileInputManager.Instance != null)
        {
            var dir = MobileInputManager.Instance.MoveInput;
            if (dir.magnitude > 0.1f)
                _lastMobileDir = dir;
        }

        if (!canAttack) return;

        // Setas do teclado (PC / editor)
        if (Input.GetKeyDown(KeyCode.UpArrow))    { lastDirection = "Up";    AttackTrigger(); }
        else if (Input.GetKeyDown(KeyCode.DownArrow))  { lastDirection = "Down";  AttackTrigger(); }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))  { lastDirection = "Left";  AttackTrigger(); }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) { lastDirection = "Right"; AttackTrigger(); }

        // Botão de ataque mobile
        if (MobileInputManager.Instance != null && MobileInputManager.Instance.ConsumeAttack())
        {
            lastDirection = VectorToDirection(_lastMobileDir);
            AttackTrigger();
        }
    }

    // Converte vetor do joystick para direção cardinal
    string VectorToDirection(Vector2 v)
    {
        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            return v.x >= 0 ? "Right" : "Left";
        return v.y > 0 ? "Up" : "Down";
    }

    public void ActivateHitbox()
    {
        CancelInvoke(nameof(DisableAll));
        DisableAll();

        if (lastDirection == "Up")    attackUp.SetActive(true);
        if (lastDirection == "Down")  attackDown.SetActive(true);
        if (lastDirection == "Left")  attackLeft.SetActive(true);
        if (lastDirection == "Right") attackRight.SetActive(true);

        Invoke(nameof(DisableAll), 0.1f);
    }

    void DisableAll()
    {
        attackUp.SetActive(false);
        attackDown.SetActive(false);
        attackLeft.SetActive(false);
        attackRight.SetActive(false);
    }

    void AttackTrigger()
    {
        Haptics.Light();
        canAttack   = false;
        isAttacking = true;

        if (lastDirection == "Left")  { sr.flipX = true;  transform.rotation = Quaternion.identity; }
        if (lastDirection == "Right") { sr.flipX = false; transform.rotation = Quaternion.identity; }
        if (lastDirection == "Up")    { sr.flipX = false; transform.rotation = Quaternion.Euler(0, 0, 90); }
        if (lastDirection == "Down")  { sr.flipX = false; transform.rotation = Quaternion.Euler(0, 0, -90); }

        anim.SetTrigger("Attack");
    }

    public void ResetAttack()
    {
        canAttack   = true;
        isAttacking = false;
        transform.rotation = Quaternion.identity;
    }

    public float GetDamage()
    {
        return stats.damage * damageMultiplier;
    }

    // ===== BOOST =====
    public void ActivateDamageBoost(float duration)
    {
        if (boostCoroutine != null) StopCoroutine(boostCoroutine);
        boostCoroutine = StartCoroutine(DamageBoostRoutine(duration));
    }

    IEnumerator DamageBoostRoutine(float duration)
    {
        damageMultiplier = 2f;

        GameObject audioObj = null;
        if (boostMusic != null)
        {
            audioObj = new GameObject("BoostMusic");
            audioObj.transform.SetParent(transform);
            AudioSource src = audioObj.AddComponent<AudioSource>();
            src.clip = boostMusic;
            src.loop = true;
            src.Play();
        }

        yield return new WaitForSeconds(duration);

        damageMultiplier = 1f;
        boostCoroutine   = null;
        if (audioObj != null) Destroy(audioObj);
    }
}
