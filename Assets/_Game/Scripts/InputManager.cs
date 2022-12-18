using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static event Action<PointerEventData> PointerDown;
        public static event Action<PointerEventData> PointerDrag;
        public static event Action<PointerEventData> PointerEnd;

        public void OnDrag(PointerEventData eventData)
        {
            PointerDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointerEnd?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            PointerDown?.Invoke(eventData);
        }
    }
}