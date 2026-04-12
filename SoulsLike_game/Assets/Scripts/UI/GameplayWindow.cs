using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class GameplayWindow : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _manaBar;
        [SerializeField] private TMP_Text _healCounterText;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        // Эти методы будут вызываться из игровой логики / MVP Presenter'a
        public void UpdateHealth(float normalizedValue)
        {
            _healthBar.fillAmount = Mathf.Clamp01(normalizedValue);
        }

        public void UpdateMana(float normalizedValue)
        {
            _manaBar.fillAmount = Mathf.Clamp01(normalizedValue);
        }

        public void UpdateHealCount(int count)
        {
            _healCounterText.text = $"Хилки: {count}";
        }
    }
}
