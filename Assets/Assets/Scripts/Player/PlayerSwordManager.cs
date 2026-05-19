using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSwordManager : MonoBehaviour
{
    public static PlayerSwordManager Instance { get; private set; }

    public SwordType equippedSword = SwordType.None;
    public bool lifeSwordBonusApplied = false;

    private GameObject labelCanvas;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (equippedSword == SwordType.Life && !lifeSwordBonusApplied)
        {
            PlayerStats ps = FindObjectOfType<PlayerStats>();
            if (ps != null)
            {
                ps.AddHealth(50);
                lifeSwordBonusApplied = true;
            }
        }

        if (equippedSword != SwordType.None && labelCanvas == null)
            CreateSwordLabel();
    }

    void CreateSwordLabel()
    {
        labelCanvas = new GameObject("SwordLabelCanvas");
        labelCanvas.transform.SetParent(transform);

        var canvas = labelCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;

        var scaler = labelCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        labelCanvas.AddComponent<GraphicRaycaster>();

        // Fundo semi-transparente de pergaminho escuro
        var bg = new GameObject("LabelBg");
        bg.transform.SetParent(labelCanvas.transform, false);
        bg.AddComponent<Image>().color = new Color(0.05f, 0.03f, 0.01f, 0.72f);
        var bgRT = bg.GetComponent<RectTransform>();
        bgRT.anchorMin = new Vector2(0f, 0f);
        bgRT.anchorMax = new Vector2(0.21f, 0.068f);
        bgRT.offsetMin = new Vector2(10f, 8f);
        bgRT.offsetMax = new Vector2(0f, 0f);

        // Faixa colorida na lateral esquerda
        var accent = new GameObject("LabelAccent");
        accent.transform.SetParent(bg.transform, false);
        accent.AddComponent<Image>().color = GetSwordColor();
        var accentRT = accent.GetComponent<RectTransform>();
        accentRT.anchorMin = Vector2.zero;
        accentRT.anchorMax = new Vector2(0.04f, 1f);
        accentRT.offsetMin = Vector2.zero;
        accentRT.offsetMax = Vector2.zero;

        // Texto com o nome da espada
        var textGo = new GameObject("LabelText");
        textGo.transform.SetParent(bg.transform, false);
        var t = textGo.AddComponent<Text>();
        t.text               = GetSwordDisplayName();
        t.font               = Resources.Load<Font>("Fonts/MedievalSharp-Regular")
                             ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize           = 21;
        t.fontStyle          = FontStyle.BoldAndItalic;
        t.alignment          = TextAnchor.MiddleLeft;
        t.color              = GetSwordColor();
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        var textRT = textGo.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0.06f, 0f);
        textRT.anchorMax = new Vector2(1f, 1f);
        textRT.offsetMin = new Vector2(8f, 0f);
        textRT.offsetMax = Vector2.zero;
    }

    string GetSwordDisplayName()
    {
        return equippedSword switch
        {
            SwordType.Fire => "Espada de Fogo",
            SwordType.Ice  => "Espada de Gelo",
            SwordType.Life => "Espada da Vida",
            _              => ""
        };
    }

    Color GetSwordColor()
    {
        return equippedSword switch
        {
            SwordType.Fire => new Color(1f,   0.50f, 0.10f),
            SwordType.Ice  => new Color(0.45f, 0.80f, 1f),
            SwordType.Life => new Color(0.35f, 1f,   0.45f),
            _              => Color.white
        };
    }
}
