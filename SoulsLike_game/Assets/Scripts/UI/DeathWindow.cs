using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeathWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _respawnButton;

        public event Action OnMainMenuClicked;
        public event Action OnRespawnClicked;

        private void Awake()
        {
            if (_mainMenuButton != null)
                _mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());
                
            if (_respawnButton != null)
                _respawnButton.onClick.AddListener(() => OnRespawnClicked?.Invoke());
        }

        private void OnDestroy()
        {
            if (_mainMenuButton != null)
                _mainMenuButton.onClick.RemoveAllListeners();
                
            if (_respawnButton != null)
                _respawnButton.onClick.RemoveAllListeners();
        }

        public void Show() 
        {
            gameObject.SetActive(true);
            if (_respawnButton != null)
                _respawnButton.Select();
            else if (_mainMenuButton != null)
                _mainMenuButton.Select();
        }
        
        public void Hide() => gameObject.SetActive(false);
    }
}
