using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool IsGameOver { get; set; }

    private bool isPaused = false;
    private GameObject pausePanel;

    static readonly Color BG     = new Color(0.04f, 0.02f, 0.01f, 0.92f);
    static readonly Color Gold   = new Color(0.92f, 0.75f, 0.18f, 1f);
    static readonly Color GoldDim= new Color(0.42f, 0.32f, 0.06f, 1f);
    static readonly Color Cream  = new Color(0.93f, 0.88f, 0.72f, 1f);
    static readonly Color BtnBg  = new Color(0.14f, 0.09f, 0.02f, 1f);

    private TMP_FontAsset font;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        IsGameOver = false;
    }

    void Start()
    {
        font = Resources.Load<TMP_FontAsset>("Fonts/MedievalSharp-Regular SDF");
        BuildUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
            Toggle();
    }

    public void Toggle()
    {
        if (isPaused) Resume(); else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // ── UI ────────────────────────────────────────────────────────────────────

    void BuildUI()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        var canvasGo = new GameObject("PauseCanvas");
        canvasGo.transform.SetParent(transform);
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200;
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGo.AddComponent<GraphicRaycaster>();

        // Overlay escuro
        pausePanel = MkImage(canvasGo.transform, "PausePanel", BG, Vector2.zero, Vector2.one);

        // Painel central
        var panel = MkImage(pausePanel.transform, "Panel", new Color(0.07f, 0.04f, 0.01f, 1f),
            new Vector2(0.35f, 0.25f), new Vector2(0.65f, 0.75f));

        // Borda dourada
        var border = panel.GetComponent<Image>();
        border.color = new Color(0.07f, 0.04f, 0.01f, 1f);

        // Linha topo
        MkImage(panel.transform, "LineTop", GoldDim,
            new Vector2(0.05f, 0.91f), new Vector2(0.95f, 0.914f));

        // Título
        MkTMP(panel.transform, "Title", "— PAUSA —",
            new Vector2(0.05f, 0.80f), new Vector2(0.95f, 0.92f),
            52, Gold, FontStyles.Bold);

        // Linha divisória
        MkImage(panel.transform, "LineMid", GoldDim,
            new Vector2(0.05f, 0.785f), new Vector2(0.95f, 0.789f));

        // Botão Continuar
        MakeButton(panel.transform, "Continuar",
            new Vector2(0.10f, 0.58f), new Vector2(0.90f, 0.72f),
            new Color(0.10f, 0.35f, 0.10f, 1f), () => Resume());

        // Botão Menu Principal
        MakeButton(panel.transform, "Menu Principal",
            new Vector2(0.10f, 0.38f), new Vector2(0.90f, 0.52f),
            new Color(0.30f, 0.12f, 0.04f, 1f), () => GoToMenu());

        // Texto de hint
        MkTMP(panel.transform, "Hint", "[ESC] para continuar",
            new Vector2(0.05f, 0.10f), new Vector2(0.95f, 0.24f),
            22, new Color(Cream.r, Cream.g, Cream.b, 0.5f), FontStyles.Italic);

        pausePanel.SetActive(false);
    }

    void MakeButton(Transform parent, string label, Vector2 aMin, Vector2 aMax, Color color, UnityEngine.Events.UnityAction onClick)
    {
        var go = new GameObject($"Btn_{label}");
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = aMin;
        rt.anchorMax = aMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        var cb = btn.colors;
        cb.normalColor      = Color.white;
        cb.highlightedColor = new Color(0.80f, 0.80f, 0.80f);
        cb.pressedColor     = new Color(0.55f, 0.55f, 0.55f);
        btn.colors = cb;

        MkTMP(go.transform, "Label", label,
            new Vector2(0.05f, 0.1f), new Vector2(0.95f, 0.9f),
            36, Cream, FontStyles.Bold);

        btn.onClick.AddListener(onClick);
    }

    GameObject MkImage(Transform parent, string name, Color color, Vector2 aMin, Vector2 aMax)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = aMin;
        rt.anchorMax = aMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        return go;
    }

    void MkTMP(Transform parent, string name, string content,
               Vector2 aMin, Vector2 aMax, float size, Color color, FontStyles style)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text      = content;
        t.font      = font;
        t.fontSize  = size;
        t.fontStyle = style;
        t.alignment = TextAlignmentOptions.Center;
        t.color     = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = aMin;
        rt.anchorMax = aMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
