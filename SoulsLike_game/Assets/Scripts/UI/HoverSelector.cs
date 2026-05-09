using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Автоматически выделяет (Select) UI-элемент при наведении мыши.
    /// Это позволяет синхронизировать управление мышью и с клавиатуры/геймпада,
    /// чтобы состояния OnHover и OnSelect работали одинаково.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class HoverSelector : MonoBehaviour, IPointerEnterHandler
    {
        private Selectable _selectable;

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_selectable && _selectable.interactable)
                _selectable.Select();
        }
    }
}
