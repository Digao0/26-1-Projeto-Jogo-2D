using UnityEngine;
using System.Collections.Generic;

public class GodRays : MonoBehaviour
{
    [Header("Quantidade")]
    public int numeroRaios = 25;

    [Header("Forma de cada raio")]
    public float comprimentoMin = 8f;
    public float comprimentoMax = 20f;
    public float larguraMin = 0.3f;
    public float larguraMax = 1.2f;

    [Header("Distribuição")]
    public float areaX = 25f;
    public float areaY = 15f;
    public float anguloRaios = -30f;

    [Header("Cor")]
    public Color corRaio = new Color(1f, 0.95f, 0.7f);
    public float alphaMin = 0.05f;
    public float alphaMax = 0.2f;

    [Header("Ciclo de vida")]
    public float duracaoMin = 2f;
    public float duracaoMax = 5f;

    void Start()
    {
        for (int i = 0; i < numeroRaios; i++)
            CriarRaio(i);
    }

    public void CriarRaio(int index)
    {
        GameObject raio = new GameObject("Raio_" + index);
        raio.transform.parent = transform;
        raio.transform.localEulerAngles = new Vector3(0, 0, anguloRaios);

        MeshFilter mf = raio.AddComponent<MeshFilter>();
        MeshRenderer mr = raio.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = corRaio;
        mr.material = mat;
        mr.sortingLayerName = "Light";
        mr.sortingOrder = 100;

        RaioVida rv = raio.AddComponent<RaioVida>();
        rv.parent = this;
        rv.Resetar();
    }
}

public class RaioVida : MonoBehaviour
{
    public GodRays parent;
    private Material mat;
    private Color corBase;
    private float alphaMax;
    private float duracao;
    private float tempo;

    public void Resetar()
    {
        float posX = Random.Range(-parent.areaX / 2f, parent.areaX / 2f);
        float posY = Random.Range(-parent.areaY / 2f, parent.areaY / 2f);
        transform.localPosition = new Vector3(posX, posY, 0);

        float comprimento = Random.Range(parent.comprimentoMin, parent.comprimentoMax);
        float largura = Random.Range(parent.larguraMin, parent.larguraMax);

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(-largura / 2f, 0, 0),
            new Vector3(largura / 2f, 0, 0),
            new Vector3(-largura / 2f, -comprimento * 0.3f, 0),
            new Vector3(largura / 2f, -comprimento * 0.3f, 0),
            new Vector3(-largura / 2f, -comprimento, 0),
            new Vector3(largura / 2f, -comprimento, 0)
        };
        mesh.colors = new Color[]
        {
            new Color(1f, 1f, 1f, 1f),
            new Color(1f, 1f, 1f, 1f),
            new Color(1f, 1f, 1f, 0.7f),
            new Color(1f, 1f, 1f, 0.7f),
            new Color(1f, 1f, 1f, 0f),
            new Color(1f, 1f, 1f, 0f)
        };
        mesh.triangles = new int[]
        {
            0, 1, 2,  2, 1, 3,
            2, 3, 4,  4, 3, 5
        };
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().mesh = mesh;

        mat = GetComponent<MeshRenderer>().material;
        corBase = parent.corRaio;
        alphaMax = Random.Range(parent.alphaMin, parent.alphaMax);
        duracao = Random.Range(parent.duracaoMin, parent.duracaoMax);
        tempo = Random.Range(0f, duracao); // começa em tempo aleatório pra não piscar tudo junto
    }

    void Update()
    {
        tempo += Time.deltaTime;
        float t = tempo / duracao;

        float alpha;
        if (t < 0.3f)
            alpha = Mathf.Lerp(0f, alphaMax, t / 0.3f);
        else if (t < 0.7f)
            alpha = alphaMax;
        else if (t < 1f)
            alpha = Mathf.Lerp(alphaMax, 0f, (t - 0.7f) / 0.3f);
        else
        {
            Resetar();
            return;
        }

        Color c = corBase;
        c.a = alpha;
        mat.color = c;
    }
}