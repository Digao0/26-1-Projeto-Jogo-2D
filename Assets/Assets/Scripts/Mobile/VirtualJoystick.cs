using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [HideInInspector] public RectTransform knob;
    [HideInInspector] public float maxRadius = 65f;
    [HideInInspector] public MobileInputManager manager;

    private RectTransform _rt;

    void Awake() => _rt = GetComponent<RectTransform>();

    public void OnPointerDown(PointerEventData e) => Move(e);
    public void OnDrag(PointerEventData e)        => Move(e);

    public void OnPointerUp(PointerEventData e)
    {
        knob.anchoredPosition = Vector2.zero;
        manager.SetMove(Vector2.zero);
    }

    void Move(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rt, e.position, e.pressEventCamera, out var local);

        var clamped = Vector2.ClampMagnitude(local, maxRadius);
        knob.anchoredPosition = clamped;
        manager.SetMove(clamped / maxRadius);
    }
}
