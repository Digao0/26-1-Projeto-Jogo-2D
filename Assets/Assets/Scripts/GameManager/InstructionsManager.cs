using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsManager : MonoBehaviour
{
    public Button botaoIniciar;
    public string nomeCenaJogo = "SampleScene";

    void Start()
    {
        if (botaoIniciar != null)
            botaoIniciar.onClick.AddListener(IniciarJogo);
    }

    void IniciarJogo()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }
}
