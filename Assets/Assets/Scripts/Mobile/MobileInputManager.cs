using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    private bool _attackPending;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        BuildUI();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetMove(Vector2 v)   => MoveInput = v;
    public void RegisterAttack()     => _attackPending = true;

    public bool ConsumeAttack()
    {
        if (!_attackPending) return false;
        _attackPending = false;
        return true;
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

        var canvasGo = new GameObject("MobileControlsCanvas");
        canvasGo.transform.SetParent(transform);

        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;

        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGo.AddComponent<GraphicRaycaster>();

        BuildJoystick(canvasGo.transform);
        BuildAttackButton(canvasGo.transform);
        BuildPauseButton(canvasGo.transform);
    }

    void BuildJoystick(Transform parent)
    {
        // Canto inferior esquerdo
        var bg = MkCircleImage(parent, "JoystickBg", new Color(0.08f, 0.08f, 0.08f, 0.50f),
            new Vector2(0, 0), new Vector2(0, 0), new Vector2(0.5f, 0.5f),
            new Vector2(200, 200), new Vector2(240, 240));

        var knob = MkCircleImage(bg.transform, "JoystickKnob", new Color(0.85f, 0.85f, 0.85f, 0.70f),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero, new Vector2(100, 100));

        var joystick    = bg.AddComponent<VirtualJoystick>();
        joystick.knob   = knob.GetComponent<RectTransform>();
        joystick.maxRadius = 65f;
        joystick.manager = this;
    }

    void BuildAttackButton(Transform parent)
    {
        var go = MkCircleImage(parent, "AttackBtn", new Color(0.82f, 0.22f, 0.04f, 0.78f),
            new Vector2(1, 0), new Vector2(1, 0), new Vector2(0.5f, 0.5f),
            new Vector2(-200, 200), new Vector2(170, 170));

        MkLabel(go.transform, "ATK", 46);

        go.AddComponent<AttackButton>().manager = this;
    }

    void BuildPauseButton(Transform parent)
    {
        var go = MkCircleImage(parent, "PauseBtn", new Color(0.08f, 0.05f, 0.01f, 0.70f),
            new Vector2(1, 1), new Vector2(1, 1), new Vector2(1f, 1f),
            new Vector2(-30, -30), new Vector2(90, 90));

        MkLabel(go.transform, "II", 32);

        var btn = go.AddComponent<Button>();
        btn.targetGraphic = go.GetComponent<Image>();
        btn.onClick.AddListener(() => { if (PauseManager.Instance != null) PauseManager.Instance.Toggle(); });
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    // Cria um Image com sprite circular gerado por código
    GameObject MkCircleImage(Transform parent, string name, Color color,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
        Vector2 anchoredPos, Vector2 size)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var img = go.AddComponent<Image>();
        img.sprite = MakeCircleSprite(128);
        img.color  = color;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = anchorMin;
        rt.anchorMax        = anchorMax;
        rt.pivot            = pivot;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta        = size;
        return go;
    }

    // Gera uma textura circular branca (fundo transparente)
    Sprite MakeCircleSprite(int resolution)
    {
        var tex    = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        var center = new Vector2(resolution / 2f, resolution / 2f);
        float r    = resolution / 2f;

        for (int y = 0; y < resolution; y++)
        for (int x = 0; x < resolution; x++)
        {
            float dist = Vector2.Distance(new Vector2(x, y), center);
            float alpha = Mathf.Clamp01(r - dist);          // borda suave de 1px
            tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
        }

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, resolution, resolution),
                             new Vector2(0.5f, 0.5f));
    }

    void MkLabel(Transform parent, string text, float fontSize)
    {
        var go  = new GameObject("Label");
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text      = text;
        tmp.fontSize  = fontSize;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color     = Color.white;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin  = Vector2.zero;
        rt.anchorMax  = Vector2.one;
        rt.offsetMin  = Vector2.zero;
        rt.offsetMax  = Vector2.zero;
    }
}
