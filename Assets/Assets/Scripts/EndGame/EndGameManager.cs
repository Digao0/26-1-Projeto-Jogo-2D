using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [Header("UI")]
    public Button botaoMenu;
    public Image imagemFinal;

    [Header("Configuração")]
    public string nomeCenaMenu = "Menu";

    void Start()
    {
        Time.timeScale = 1f;
        if (botaoMenu != null)
            botaoMenu.onClick.AddListener(VoltarMenu);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(nomeCenaMenu);
    }
}
