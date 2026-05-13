using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                PlayerAttack pa = GetComponentInParent<PlayerAttack>();
                float mult = pa != null ? pa.damageMultiplier : 1f;
                enemy.TakeDamage(Mathf.RoundToInt(damage * mult), transform);
            }
        }
    }
}