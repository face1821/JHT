using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Maxy.GameFramework.MobileGame.UI
{
    public class EasyFingerTrigger : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UnityEvent OnPointerClickEvent;
        [SerializeField] private UnityEvent OnPointerDownEvent;
        [SerializeField] private UnityEvent OnPointerUpEvent;
        [SerializeField] private UnityEvent OnPointerEnterEvent;
        [SerializeField] private UnityEvent OnPointerExitEvent;

        public void OnPointerClick(PointerEventData eventData) { OnPointerClickEvent?.Invoke(); }

        public void OnPointerDown(PointerEventData eventData) { OnPointerDownEvent?.Invoke(); }

        public void OnPointerUp(PointerEventData eventData) { OnPointerUpEvent?.Invoke(); }

        public void OnPointerEnter(PointerEventData eventData) { OnPointerEnterEvent?.Invoke(); }

        public void OnPointerExit(PointerEventData eventData) { OnPointerExitEvent?.Invoke(); }
    }
}