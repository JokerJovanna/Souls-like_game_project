using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class GameplayWindow : MonoBehaviour
    {
        // Singleton instance for global access
        public static GameplayWindow Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _manaBar;
        [SerializeField] private TMP_Text _healCounterText;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}