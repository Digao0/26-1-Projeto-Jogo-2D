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

        int damage = Mathf.RoundToInt(playerAttack.GetDamage());

        // Espada da Vida: adiciona 40% da vida atual como dano bônus
        if (PlayerSwordManager.Instance != null && PlayerSwordManager.Instance.equippedSword == SwordType.Life)
        {
            PlayerHealth ph = GetComponentInParent<PlayerHealth>();
            if (ph != null)
                damage += Mathf.RoundToInt(ph.GetCurrentHealth() * 0.4f);
        }

        enemy.TakeDamage(damage, transform);
        DamageNumber.Spawn(collision.transform.position, damage, Color.red);

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