using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackUp;
    public GameObject attackDown;
    public GameObject attackLeft;
    public GameObject attackRight;
    private Animator anim;
    private bool canAttack = true;
    SpriteRenderer sr;
    public bool isAttacking = false;

    private string lastDirection = "Down";

    [Header("Power-up de Dano")]
    public float damageMultiplier = 1f;
    public AudioClip boostMusic;
    private Coroutine boostCoroutine;

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
        boostCoroutine = null;
        if (audioObj != null) Destroy(audioObj);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (!canAttack) return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastDirection = "Up";
            AttackTrigger();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastDirection = "Down";
            AttackTrigger();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastDirection = "Left";
            AttackTrigger();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastDirection = "Right";
            AttackTrigger();
        }
    }

    void DisableAll()
    {
        attackUp.SetActive(false);
        attackDown.SetActive(false);
        attackLeft.SetActive(false);
        attackRight.SetActive(false);
    }
    
    public void ActivateHitbox()
    {
        CancelInvoke(nameof(DisableAll));
        DisableAll();

        if (lastDirection == "Up") attackUp.SetActive(true);
        if (lastDirection == "Down") attackDown.SetActive(true);
        if (lastDirection == "Left") attackLeft.SetActive(true);
        if (lastDirection == "Right") attackRight.SetActive(true);

        Invoke(nameof(DisableAll), 0.1f);
    }
    
    void AttackTrigger()
    {
        canAttack = false;
        isAttacking = true;

        if (lastDirection == "Left")  { sr.flipX = true;  transform.rotation = Quaternion.identity; }
        if (lastDirection == "Right") { sr.flipX = false; transform.rotation = Quaternion.identity; }
        if (lastDirection == "Up")    { sr.flipX = false; transform.rotation = Quaternion.Euler(0, 0, 90); }
        if (lastDirection == "Down")  { sr.flipX = false; transform.rotation = Quaternion.Euler(0, 0, -90); }

        anim.SetTrigger("Attack");
    }

    public void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
        transform.rotation = Quaternion.identity;
    }
}