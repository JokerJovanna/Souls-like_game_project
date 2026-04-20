using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        public event Action OnPlayClicked;
        public event Action OnExitClicked;

        private void Awake()
        {
            _playButton.onClick.AddListener(() => OnPlayClicked?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

        public void Show() 
        {
            gameObject.SetActive(true);
            _playButton.Select();
        }
        
        public void Hide() => gameObject.SetActive(false);
    }
}
