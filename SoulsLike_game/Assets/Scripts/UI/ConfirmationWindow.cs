using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class ConfirmationWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private Action _onConfirm;
        private Action _onCancel;

        private void Awake()
        {
            _yesButton.onClick.AddListener(Confirm);
            _noButton.onClick.AddListener(Cancel);
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }

        public void Show(string message, Action onConfirm, Action onCancel)
        {
            _messageText.text = message;
            _onConfirm = onConfirm;
            _onCancel = onCancel;
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        private void Confirm()
        {
            Hide();
            _onConfirm?.Invoke();
        }

        private void Cancel()
        {
            Hide();
            _onCancel?.Invoke();
        }
    }
}
