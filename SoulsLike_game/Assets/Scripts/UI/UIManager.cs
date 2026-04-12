using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuWindow _mainMenu;
        [SerializeField] private GameplayWindow _gameplayUI;
        [SerializeField] private PauseMenuWindow _pauseMenu;
        [SerializeField] private ConfirmationWindow _confirmationMenu;

        private enum UIState { MainMenu, Gameplay, Paused }
        private UIState _currentState;

        private void Awake()
        {
            SubscribeToEvents();
        }

        private void Start()
        {
            OpenMainMenu();
        }

        private void Update()
        {
            // Используем новую систему ввода (Input System Package)
            if (_currentState == UIState.Gameplay && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                OpenPauseMenu();
            }
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
            _currentState = UIState.MainMenu;
            CloseAllWindows();
            _mainMenu.Show();
        }

        private void StartGame()
        {
            _currentState = UIState.Gameplay;
            CloseAllWindows();
            _gameplayUI.Show();
        }

        private void OpenPauseMenu()
        {
            _currentState = UIState.Paused;
            _pauseMenu.Show();
        }

        private void ResumeGame()
        {
            _currentState = UIState.Gameplay;
            _pauseMenu.Hide();
        }

        private void RequestExitToMainMenu()
        {
            _confirmationMenu.Show("Выйти в главное меню без сохранения?", 
                onConfirm: OpenMainMenu, 
                onCancel: () => { /* ничего не делаем, окно само скроется */ });
        }

        private void RequestExitFromApp()
        {
            _confirmationMenu.Show("Вы уверены что хотите выйти?", 
                onConfirm: () => 
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }, 
                onCancel: () => { /* окно скроется */ });
        }
    }
}
