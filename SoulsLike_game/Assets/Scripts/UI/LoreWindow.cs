using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoreWindow : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;

        public event Action OnContinueClicked;

        private void Awake()
        {
            if (_continueButton != null)
                _continueButton.onClick.AddListener(() => OnContinueClicked?.Invoke());
        }

        private void OnDestroy()
        {
            if (_continueButton != null)
                _continueButton.onClick.RemoveAllListeners();
        }

        public void Show() 
        {
            gameObject.SetActive(true);
            if (_continueButton != null)
                _continueButton.Select();
        }
        
        public void Hide() => gameObject.SetActive(false);
    }
}
