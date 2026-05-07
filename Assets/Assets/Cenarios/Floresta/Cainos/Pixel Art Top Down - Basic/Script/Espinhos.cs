using UnityEngine;

public class Espinho : MonoBehaviour
{
    public int dano = 10;
    public float intervaloDano = 1f;
    private float ultimoDano;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AplicarDano(other);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= ultimoDano + intervaloDano)
            {
                AplicarDano(other);
            }
        }
    }

    void AplicarDano(Collider2D other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(dano, transform);
            ultimoDano = Time.time;
        }
    }
}