using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _mainMenuButton;

        public event Action OnContinueClicked;
        public event Action OnMainMenuClicked;

        private void Awake()
        {
            _continueButton.onClick.AddListener(() => OnContinueClicked?.Invoke());
            _mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveAllListeners();
            _mainMenuButton.onClick.RemoveAllListeners();
        }

        public void Show() 
        {
            gameObject.SetActive(true);
            _continueButton.Select();
        }
        
        public void Hide() => gameObject.SetActive(false);
    }
}
