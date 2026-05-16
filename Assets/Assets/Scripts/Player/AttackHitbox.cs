using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy == null) return;

        PlayerAttack pa = GetComponentInParent<PlayerAttack>();
        float mult = pa != null ? pa.damageMultiplier : 1f;
        enemy.TakeDamage(Mathf.RoundToInt(damage * mult), transform);

        if (PlayerSwordManager.Instance == null) return;

        switch (PlayerSwordManager.Instance.equippedSword)
        {
            case SwordType.Fire:
                enemy.ApplyBurn(5, 3f);
                break;
            case SwordType.Ice:
                collision.GetComponent<EnemyFollow>()?.ApplySlow(0.4f, 2f);
                break;
        }
    }
}