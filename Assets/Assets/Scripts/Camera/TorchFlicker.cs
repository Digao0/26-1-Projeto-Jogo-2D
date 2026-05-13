using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    public Light2D luz;
    public float intensidadeMin = 1.0f;
    public float intensidadeMax = 1.4f;
    public float velocidade = 8f;

    void Start()
    {
        if (luz == null) luz = GetComponent<Light2D>();
    }

    void Update()
    {
        float ruido = Mathf.PerlinNoise(Time.time * velocidade, 0f);
        luz.intensity = Mathf.Lerp(intensidadeMin, intensidadeMax, ruido);
    }
}