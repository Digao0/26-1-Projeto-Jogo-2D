using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public MobileInputManager manager;

    public void OnPointerDown(PointerEventData e) => manager.RegisterAttack();
}
