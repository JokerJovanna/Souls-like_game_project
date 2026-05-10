using UnityEngine;
using TMPro;

namespace UI
{
    public class TutorialOverlay : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _text;

        [Header("Настройки")]
        [SerializeField] private KeyCode[] _keys;
        [SerializeField] private string[] _messages;

        private int _current = 0;
        private bool _visible = false;

        private void Awake()
        {
            if (_messages == null || _messages.Length == 0)
                _messages = new[] { "Press <b>Space</b> to jump.", "Press <b>WASD</b> to move.", "Enjoy the game!" };
            if (_keys == null || _keys.Length == 0)
                _keys = new[] { KeyCode.Space };
            Hide();
        }

        private void Update()
        {
            if (!_visible) return;

            foreach (var k in _keys)
            {
                if (Input.GetKeyDown(k))
                {
                    _current = (_current + 1) % _messages.Length;
                    Refresh();
                    break;
                }
            }
        }

        private void Refresh()
        {
            _text.text = _messages[_current];
        }

        public void Show()
        {
            _visible = true;
            gameObject.SetActive(true);
            Refresh();
        }

        public void Hide()
        {
            _visible = false;
            gameObject.SetActive(false);
        }
    }
}
