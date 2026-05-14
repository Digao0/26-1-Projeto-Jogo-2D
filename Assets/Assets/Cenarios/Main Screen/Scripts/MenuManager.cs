using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Button botaoIniciar;

    [Header("Configuracao")]
    public string nomeCenaProxima = "SampleScene";

    void Start()
    {
        botaoIniciar.onClick.AddListener(Iniciar);
    }

    void Iniciar()
    {
        SceneManager.LoadScene(nomeCenaProxima);
    }
}