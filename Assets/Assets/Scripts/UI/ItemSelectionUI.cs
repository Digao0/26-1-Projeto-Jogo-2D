using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ItemSelectionUI : MonoBehaviour
{
    private string nextScene;

    // Paleta medieval
    static readonly Color BG          = new Color(0.035f, 0.020f, 0.005f, 0.96f);
    static readonly Color FrameGold   = new Color(0.55f,  0.42f,  0.07f,  1f);
    static readonly Color PanelDark   = new Color(0.09f,  0.060f, 0.025f, 1f);
    static readonly Color PanelLight  = new Color(0.16f,  0.110f, 0.045f, 1f);
    static readonly Color Gold        = new Color(0.92f,  0.750f, 0.18f,  1f);
    static readonly Color GoldDim     = new Color(0.52f,  0.400f, 0.08f,  1f);
    static readonly Color Cream       = new Color(0.93f,  0.880f, 0.72f,  1f);
    static readonly Color CreamDim    = new Color(0.58f,  0.530f, 0.40f,  1f);

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

        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        gameObject.AddComponent<GraphicRaycaster>();

        // Fundo escuro
        var bg = MkPanel(transform, "BG", BG, Vector2.zero, Vector2.one);

        // Moldura dourada externa
        var frame = MkPanel(bg.transform, "Frame", FrameGold,
            new Vector2(0.03f, 0.035f), new Vector2(0.97f, 0.965f));

        // Painel interno de pergaminho escuro
        var panel = MkInner(frame, "Panel", PanelDark, 3f);

        // Faixa dourada topo
        MkPanel(panel.transform, "TopStrip", GoldDim,
            new Vector2(0.04f, 0.915f), new Vector2(0.96f, 0.921f));

        // Título
        MkTxt(panel.transform, "Title", "Escolha sua Espada",
            new Vector2(0.04f, 0.835f), new Vector2(0.96f, 0.970f),
            54, Gold, FontStyle.Bold);

        // Linha divisória abaixo do título
        MkPanel(panel.transform, "TitleDiv", GoldDim,
            new Vector2(0.04f, 0.803f), new Vector2(0.96f, 0.808f));

        // Subtítulo
        MkTxt(panel.transform, "Subtitle",
            "O poder escolhido acompanhara voce ate o fim da jornada",
            new Vector2(0.05f, 0.742f), new Vector2(0.95f, 0.806f),
            21, CreamDim, FontStyle.Italic);

        // Linha divisória abaixo do subtítulo
        MkPanel(panel.transform, "SubDiv", GoldDim,
            new Vector2(0.04f, 0.722f), new Vector2(0.96f, 0.727f));

        // Cards
        MakeCard(panel.transform, 0,
            "Espada de Fogo",
            "Ao atacar, queima o inimigo\ncausando dano extra por 3 segundos.",
            "FOGO",
            new Color(0.78f, 0.22f, 0.04f), SwordType.Fire);

        MakeCard(panel.transform, 1,
            "Espada de Gelo",
            "Ao atacar, congela o inimigo\nreduzindo sua velocidade por 2 segundos.",
            "GELO",
            new Color(0.22f, 0.42f, 0.74f), SwordType.Ice);

        MakeCard(panel.transform, 2,
            "Espada da Vida",
            "Aumenta sua vida maxima\nem 50 permanentemente.",
            "VIDA",
            new Color(0.12f, 0.52f, 0.18f), SwordType.Life);

        // Faixa dourada base
        MkPanel(panel.transform, "BotStrip", GoldDim,
            new Vector2(0.04f, 0.082f), new Vector2(0.96f, 0.087f));
    }

    void MakeCard(Transform parent, int index, string title, string desc,
                  string typeLabel, Color accent, SwordType type)
    {
        float xStart = 0.07f + index * 0.30f;
        float xEnd   = xStart + 0.26f;

        // Borda colorida da espada
        var border = MkPanel(parent, $"Border{index}", accent,
            new Vector2(xStart, 0.120f), new Vector2(xEnd, 0.715f));

        // Corpo do card (inset da borda)
        var card = MkInner(border, $"Card{index}", PanelDark, 2.5f);

        // ── Cabeçalho ────────────────────────────────────
        var hdr = MkPanel(card.transform, "Header", PanelLight,
            new Vector2(0f, 0.77f), new Vector2(1f, 1f));

        // Faixa colorida no topo do cabeçalho
        MkPanel(hdr.transform, "AccentStripe", accent,
            new Vector2(0f, 0.86f), new Vector2(1f, 1f));

        // Tipo da espada
        MkTxt(hdr.transform, "Badge", $"[ {typeLabel} ]",
            new Vector2(0f, 0.55f), new Vector2(1f, 0.87f),
            16, accent, FontStyle.Bold);

        // Nome da espada
        MkTxt(hdr.transform, "CardTitle", title,
            new Vector2(0.04f, 0.04f), new Vector2(0.96f, 0.57f),
            22, Gold, FontStyle.Bold);

        // Linha divisória abaixo do cabeçalho
        MkPanel(card.transform, "HdrDiv", GoldDim,
            new Vector2(0.04f, 0.750f), new Vector2(0.96f, 0.756f));

        // ── Descrição ────────────────────────────────────
        MkTxt(card.transform, "Desc", desc,
            new Vector2(0.06f, 0.42f), new Vector2(0.94f, 0.745f),
            16, Cream, FontStyle.Italic);

        // Linha acima do botão
        MkPanel(card.transform, "BtnDiv", GoldDim,
            new Vector2(0.04f, 0.305f), new Vector2(0.96f, 0.311f));

        // ── Botão ────────────────────────────────────────
        // Borda dourada do botão
        var btnBorder = MkPanel(card.transform, "BtnBorder", GoldDim,
            new Vector2(0.09f, 0.075f), new Vector2(0.91f, 0.275f));

        // Interior escuro do botão
        var btnInner = MkInner(btnBorder, "BtnInner",
            new Color(0.05f, 0.035f, 0.01f), 2f);

        var btn = btnInner.AddComponent<Button>();
        btn.targetGraphic = btnInner.GetComponent<Image>();

        var cb = btn.colors;
        cb.normalColor      = new Color(0.05f, 0.035f, 0.01f);
        cb.highlightedColor = new Color(0.25f, 0.17f,  0.04f);
        cb.pressedColor     = new Color(0.02f, 0.015f, 0.005f);
        btn.colors = cb;

        MkTxt(btnInner.transform, "BtnLabel", "[ Escolher ]",
            Vector2.zero, Vector2.one, 18, Gold, FontStyle.Bold);

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

    // ── Helpers ──────────────────────────────────────────────────────

    GameObject MkPanel(Transform parent, string name, Color color, Vector2 aMin, Vector2 aMax)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<Image>().color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        return go;
    }

    GameObject MkInner(GameObject parent, string name, Color color, float inset)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<Image>().color = color;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(inset, inset); rt.offsetMax = new Vector2(-inset, -inset);
        return go;
    }

    void MkTxt(Transform parent, string name, string content,
               Vector2 aMin, Vector2 aMax, int size, Color color, FontStyle style)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<Text>();
        t.text               = content;
        t.font               = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize           = size;
        t.fontStyle          = style;
        t.alignment          = TextAnchor.MiddleCenter;
        t.color              = color;
        t.horizontalOverflow = HorizontalWrapMode.Wrap;
        t.verticalOverflow   = VerticalWrapMode.Overflow;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = aMin; rt.anchorMax = aMax;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
    }
}
