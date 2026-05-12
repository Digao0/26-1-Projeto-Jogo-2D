using UnityEngine;

public class Espinho : MonoBehaviour
{
    public int dano = 10;
    public float intervaloDano = 1f;
    public float fatorLentidao = 0.4f;
    public float duracaoLentidao = 0.5f;
    private float ultimoDano;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            AplicarDano(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= ultimoDano + intervaloDano)
                AplicarDano(other);
        }
    }

    void AplicarDano(Collider2D other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        PlayerMovement pm = other.GetComponent<PlayerMovement>();

        if (ph != null)
        {
            ph.TakeDamageNoKnockback(dano);
            ultimoDano = Time.time;
        }

        if (pm != null)
            pm.AplicarLentidao(fatorLentidao, duracaoLentidao);
    }
}