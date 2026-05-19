using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSelectionUI : MonoBehaviour
{
    private string nextScene;

    static readonly Color BG      = new Color(0.04f, 0.02f, 0.01f, 0.96f);
    static readonly Color Gold    = new Color(0.92f, 0.75f, 0.18f, 1f);
    static readonly Color GoldDim = new Color(0.42f, 0.32f, 0.06f, 1f);
    static readonly Color Cream   = new Color(0.93f, 0.88f, 0.72f, 1f);
    static readonly Color CardBg  = new Color(0.09f, 0.06f, 0.02f, 1f);

    private TMP_FontAsset font;
    private Sprite buttonSprite;

    public static void Show(string sceneToLoad)
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        var go = new GameObject("ItemSelectionUI");
        var ui = go.AddComponent<ItemSelectionUI>();
        ui.nextScene = sceneToLoad;
        ui.Build();
    }

    void Build()
    {
        Time.timeScale = 0f;

        font = Resources.Load<TMP_FontAsset>("Fonts/MedievalSharp-Regular SDF");

        var allSprites = Resources.LoadAll<Sprite>("Sprites/login_button");
        foreach (var s in allSprites)
            if (s.name == "login_button_0") { buttonSprite = s; break; }

        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        gameObject.AddComponent<GraphicRaycaster>();

        var bg = MkImage(transform, "BG", BG, Vector2.zero, Vector2.one);

        MkTMP(bg.transform, "Title", "Escolha sua Espada",
            new Vector2(0.05f, 0.865f), new Vector2(0.95f, 0.975f),
            72, Gold, FontStyles.Bold);

        MkImage(bg.transform, "LineTitulo", GoldDim,
            new Vector2(0.05f, 0.858f), new Vector2(0.95f, 0.862f));

        MkTMP(bg.transform, "Subtitle", "O poder escolhido acompanhará você até o fim da jornada",
            new Vector2(0.10f, 0.800f), new Vector2(0.90f, 0.856f),
            28, Cream, FontStyles.Italic);

        MkImage(bg.transform, "LineSub", GoldDim,
            new Vector2(0.05f, 0.793f), new Vector2(0.95f, 0.797f));

        MakeCard(bg.transform, 0, "Espada de Fogo",
            "Ao atacar, queima o inimigo\ncausando dano extra por 3 segundos.",
            new Color(0.78f, 0.22f, 0.04f), SwordType.Fire);

        MakeCard(bg.transform, 1, "Espada de Gelo",
            "Ao atacar, congela o inimigo\nreduzindo sua velocidade por 2 segundos.",
            new Color(0.22f, 0.42f, 0.74f), SwordType.Ice);

        MakeCard(bg.transform, 2, "Espada da Vida",
            "Aumenta vida máxima em +50.\nAo atacar, causa dano bônus\nigual a 40% da sua vida atual.",
            new Color(0.12f, 0.52f, 0.18f), SwordType.Life);
    }

    void MakeCard(Transform parent, int index, string title, string desc, Color accent, SwordType type)
    {
        float cardW  = 0.26f;
        float xStart = 0.07f + index * 0.30f;
        float xEnd   = xStart + cardW;

        var border = MkImage(parent, $"Border{index}", accent,
            new Vector2(xStart, 0.10f), new Vector2(xEnd, 0.78f));

        var card = MkImage(border.transform, $"Card{index}", CardBg, Vector2.zero, Vector2.one);
        var cardRT = card.GetComponent<RectTransform>();
        cardRT.offsetMin = new Vector2(3, 3);
        cardRT.offsetMax = new Vector2(-3, -3);

        // Cabeçalho colorido
        MkImage(card.transform, "Header", accent,
            new Vector2(0f, 0.78f), new Vector2(1f, 1f));

        MkTMP(card.transform, "CardTitle", title,
            new Vector2(0.04f, 0.56f), new Vector2(0.96f, 0.78f),
            38, Gold, FontStyles.Bold);

        MkImage(card.transform, "Div", GoldDim,
            new Vector2(0.04f, 0.548f), new Vector2(0.96f, 0.553f));

        MkTMP(card.transform, "Desc", desc,
            new Vector2(0.05f, 0.27f), new Vector2(0.95f, 0.548f),
            26, Cream, FontStyles.Italic);

        MakeButton(card.transform, type);
    }

    void MakeButton(Transform parent, SwordType type)
    {
        var btnGo = new GameObject("Btn");
        btnGo.transform.SetParent(parent, false);

        var btnImg = btnGo.AddComponent<Image>();
        if (buttonSprite != null)
        {
            btnImg.sprite = buttonSprite;
            btnImg.type   = Image.Type.Simple;
        }
        else
        {
            btnImg.color = new Color(0.3f, 0.2f, 0.05f);
        }

        var btnRT = btnGo.GetComponent<RectTransform>();
        btnRT.anchorMin = new Vector2(0.08f, 0.055f);
        btnRT.anchorMax = new Vector2(0.92f, 0.235f);
        btnRT.offsetMin = Vector2.zero;
        btnRT.offsetMax = Vector2.zero;

        var btn = btnGo.AddComponent<Button>();
        btn.targetGraphic = btnImg;

        var cb = btn.colors;
        cb.normalColor      = Color.white;
        cb.highlightedColor = new Color(0.85f, 0.85f, 0.85f);
        cb.pressedColor     = new Color(0.65f, 0.65f, 0.65f);
        btn.colors = cb;

        MkTMP(btnGo.transform, "BtnLabel", "[ Escolher ]",
            Vector2.zero, Vector2.one, 42, Color.black, FontStyles.Bold);

        SwordType captured = type;
        btn.onClick.AddListener(() => OnPick(captured));
    }

    void OnPick(SwordType type)
    {
        if (PlayerSwordManager.Instance == null)
            new GameObject("PlayerSwordManager").AddComponent<PlayerSwordManager>();

        PlayerSwordManager.Instance.equippedSword = type;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextScene);
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
