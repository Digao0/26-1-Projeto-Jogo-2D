using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        PauseManager.IsGameOver = true;

        if (PauseManager.Instance != null)
            PauseManager.Instance.Resume();

        gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        PauseManager.IsGameOver = false;

        if (PlayerSwordManager.Instance != null)
        {
            PlayerSwordManager.Instance.equippedSword = SwordType.None;
            PlayerSwordManager.Instance.lifeSwordBonusApplied = false;
        }

        PlayerStats.ResetBonuses();
        PlayerHealth.ResetSavedHealth();

        SceneManager.LoadScene("SampleScene");
    }
}