using UnityEngine;

public class DamagePowerUp : MonoBehaviour
{
    public float duration = 10f;
    public AudioClip pickupSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerAttack pa = other.GetComponent<PlayerAttack>();
        if (pa == null) return;

        pa.ActivateDamageBoost(duration);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}
