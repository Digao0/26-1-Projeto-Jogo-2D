using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    private bool isGameOver = false;
    private GameObject _restartBtn;

    static readonly Color BtnGreen = new Color(0.10f, 0.35f, 0.10f, 1f);
    static readonly Color Cream    = new Color(0.93f, 0.88f, 0.72f, 1f);

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }

    public void GameOver()
    {
        isGameOver = true;
        PauseManager.IsGameOver = true;

        if (PauseManager.Instance != null)
            PauseManager.Instance.Resume();

        gameOverUI.SetActive(true);
        ShowRestartButton();

        Time.timeScale = 0f;
    }

    void ShowRestartButton()
    {
        if (_restartBtn != null) { _restartBtn.SetActive(true); return; }

        var canvas = gameOverUI.GetComponentInParent<Canvas>();
        if (canvas == null) return;

        _restartBtn = new GameObject("RestartBtn");
        _restartBtn.transform.SetParent(canvas.transform, false);

        var img = _restartBtn.AddComponent<Image>();
        img.color = BtnGreen;

        var rt = _restartBtn.GetComponent<RectTransform>();
        rt.anchorMin  = new Vector2(0.35f, 0.38f);
        rt.anchorMax  = new Vector2(0.65f, 0.50f);
        rt.offsetMin  = Vector2.zero;
        rt.offsetMax  = Vector2.zero;

        var btn = _restartBtn.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(RestartGame);

        var labelGo = new GameObject("Label");
        labelGo.transform.SetParent(_restartBtn.transform, false);
        var tmp = labelGo.AddComponent<TextMeshProUGUI>();
        tmp.text      = "Jogar Novamente";
        tmp.fontSize  = 36;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color     = Cream;
        var font = Resources.Load<TMP_FontAsset>("Fonts/MedievalSharp-Regular SDF");
        if (font != null) tmp.font = font;
        var lrt = labelGo.GetComponent<RectTransform>();
        lrt.anchorMin = Vector2.zero;
        lrt.anchorMax = Vector2.one;
        lrt.offsetMin = Vector2.zero;
        lrt.offsetMax = Vector2.zero;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        PauseManager.IsGameOver = false;
        isGameOver = false;

        if (_restartBtn != null) _restartBtn.SetActive(false);

        if (PlayerSwordManager.Instance != null)
        {
            PlayerSwordManager.Instance.equippedSword       = SwordType.None;
            PlayerSwordManager.Instance.lifeSwordBonusApplied = false;
        }

        PlayerStats.ResetBonuses();
        PlayerHealth.ResetSavedHealth();

        SceneManager.LoadScene("SampleScene");
    }
}
