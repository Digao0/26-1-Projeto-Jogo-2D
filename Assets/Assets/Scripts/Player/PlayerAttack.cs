using UnityEngine;

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
        
        if (lastDirection == "Left") sr.flipX = true;
        if (lastDirection == "Right") sr.flipX = false;
        
        anim.SetTrigger("Attack");
    }

    public void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
    }
}