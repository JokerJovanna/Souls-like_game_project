using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuWindow _mainMenu;
        [SerializeField] private GameplayWindow _gameplayUI;
        [SerializeField] private PauseMenuWindow _pauseMenu;
        [SerializeField] private ConfirmationWindow _confirmationMenu;
        [SerializeField] private Sprite _mainMenuExitBackground;
        [SerializeField] private Player _Player;
        private enum UIState { MainMenu, Gameplay, Paused, Confirmation }
        private UIState _currentState;
        private UIState _previousState; // Храним предыдущее состояние для возврата(ESC)
        private GameObject _lastSelectedGameObject; // Сохраняем последний выбранный (фокусный) элемент интерфейса

        private void Awake()
        {
            SubscribeToEvents();
        }

        private void Start()
        {
            if (_Player)
                _Player.gameObject.SetActive(false);
            OpenMainMenu();
        }

        private void Update()
        {
            EnsureUISelection();

            // Возвращаем старую систему ввода - слушаем кнопку ESC!
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandleEscapeNavigation();
            }
        }

        private void EnsureUISelection()
        {
            if (!EventSystem.current) return;

            // Обновляем ссылку, если игрок выбрал новый элемент интерфейса.
            if (EventSystem.current.currentSelectedGameObject)
                _lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            // Если игрок кликнул мимо кнопок (пустое место), возвращаем фокус на последний выделенный элемент.
            else if (_lastSelectedGameObject && _lastSelectedGameObject.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(_lastSelectedGameObject);
        }

        private void HandleEscapeNavigation()
        {
            // Навигация назад (ESC) зависит от текущего открытого окна
            if (_currentState == UIState.Gameplay)
                OpenPauseMenu();
            else if (_currentState == UIState.Paused)
                ResumeGame();
            else if (_currentState == UIState.Confirmation)
                _confirmationMenu.CallCancel();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            _mainMenu.OnPlayClicked += StartGame;
            _mainMenu.OnExitClicked += RequestExitFromApp;
            
            _pauseMenu.OnContinueClicked += ResumeGame;
            _pauseMenu.OnMainMenuClicked += RequestExitToMainMenu;
        }

        private void UnsubscribeFromEvents()
        {
            _mainMenu.OnPlayClicked -= StartGame;
            _mainMenu.OnExitClicked -= RequestExitFromApp;

            _pauseMenu.OnContinueClicked -= ResumeGame;
            _pauseMenu.OnMainMenuClicked -= RequestExitToMainMenu;
        }

        private void CloseAllWindows()
        {
            _mainMenu.Hide();
            _gameplayUI.Hide();
            _pauseMenu.Hide();
            _confirmationMenu.Hide();
        }

        private void OpenMainMenu()
        {
            _previousState = _currentState;
            _currentState = UIState.MainMenu;
            CloseAllWindows();
            Time.timeScale = 1f;
            _mainMenu.Show();
        }

        private void StartGame()
        {
            _previousState = _currentState;
            _currentState = UIState.Gameplay;
            CloseAllWindows();
            Time.timeScale = 1f;
            if (_Player)
                EnablePlayer();
            _gameplayUI.Show();
        }

        private void OpenPauseMenu()
        {
            _previousState = _currentState;
            _currentState = UIState.Paused;
            Time.timeScale = 0f;
            if (_Player)
                _Player.gameObject.SetActive(false);
            _pauseMenu.Show();
        }

        private void ResumeGame()
        {
            _previousState = _currentState;
            _currentState = UIState.Gameplay;
            Time.timeScale = 1f;
            if (_Player)
                EnablePlayer();
            _pauseMenu.Hide();
        }

        private void RequestExitToMainMenu()
        {
            _previousState = _currentState; // Запоминаем, что мы были в паузе
            _currentState = UIState.Confirmation;
            _confirmationMenu.Show(
                onConfirm: OpenMainMenu,
                onCancel: RestoreStateAfterConfirmation,
                background: _mainMenuExitBackground);
        }

        private void RequestExitFromApp()
        {
            _previousState = _currentState; // Запоминаем, что мы были в меню
            _currentState = UIState.Confirmation;
            _confirmationMenu.Show(
                onConfirm: () =>
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                },
                onCancel: RestoreStateAfterConfirmation,
                background: _mainMenuExitBackground);
        }

        private void RestoreStateAfterConfirmation()
        {
            // Возвращаемся в состояние, из которого вызвали confirmation
            _currentState = _previousState;
            // Если отменили выход из главного меню - возвращаем фокус кнопкам меню
            if (_currentState == UIState.MainMenu) _mainMenu.Show();
            // Если отменили выход из паузы - возвращаем фокус кнопкам паузы
            if (_currentState == UIState.Paused) _pauseMenu.Show();
        }

        private void EnablePlayer()
        {
            _Player.gameObject.SetActive(true);
            _Player.DisableCollisionWithEnemies();
        }
    }
}
