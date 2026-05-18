using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy == null) return;

        // 🔥 usa dano real do player (com upgrades)
        int damage = Mathf.RoundToInt(playerAttack.GetDamage());

        enemy.TakeDamage(damage, transform);

        // mostra número de dano
        DamageNumber.Spawn(collision.transform.position, damage, Color.red);

        // ===== EFEITOS DA ESPADA =====
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