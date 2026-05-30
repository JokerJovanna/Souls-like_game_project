using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeathWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;

        public event Action OnMainMenuClicked;

        private void Awake()
        {
            if (_mainMenuButton != null)
                _mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());
        }

        private void OnDestroy()
        {
            if (_mainMenuButton != null)
                _mainMenuButton.onClick.RemoveAllListeners();
        }

        public void Show() 
        {
            gameObject.SetActive(true);
            if (_mainMenuButton != null)
                _mainMenuButton.Select();
        }
        
        public void Hide() => gameObject.SetActive(false);
    }
}
