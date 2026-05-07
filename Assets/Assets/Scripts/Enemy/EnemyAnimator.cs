using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        anim.SetFloat("Speed", rb.linearVelocity.magnitude);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("Hit");
    }

    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void PlayDeath(float delay)
    {
        anim.SetTrigger("Die");
        StartCoroutine(DestroyAfterDelay(delay));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
