using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuWindow _mainMenu;
        [SerializeField] private GameplayWindow _gameplayUI;
        [SerializeField] private PauseMenuWindow _pauseMenu;
        [SerializeField] private ConfirmationWindow _confirmationMenu;
        [SerializeField] private LoreWindow _loreWindow;
        [SerializeField] private DeathWindow _deathWindow;
        [SerializeField] private Sprite _mainMenuExitBackground;
        [SerializeField] private Player _Player;
        [SerializeField] private MusicManager _backgroundMusic;

        private PlayerHealthComponent _playerHealth;
        private static bool _loreShownThisSession = false;
        private static bool _shouldAutoStartGame = false;

        private enum UIState
        {
            MainMenu,
            Gameplay,
            Paused,
            Confirmation,
            Lore,
            Death
        }

        private UIState _currentState;
        private UIState _previousState;
        private GameObject _lastSelectedGameObject;

        private void Awake()
        {
            if (_Player != null)
                _playerHealth = _Player.GetComponent<PlayerHealthComponent>();
            SubscribeToEvents();
        }

        private void Start()
        {
            if (_shouldAutoStartGame)
            {
                _shouldAutoStartGame = false;
                _loreShownThisSession = true;
                StartGame();
            }
            else
            {
                if (_Player)
                    _Player.gameObject.SetActive(false);

                OpenMainMenu();
            }
        }

        private void Update()
        {
            EnsureUISelection();
            if (Input.GetKeyDown(KeyCode.Escape))
                HandleEscapeNavigation();
        }

        private void EnsureUISelection()
        {
            if (!EventSystem.current) return;
            if (EventSystem.current.currentSelectedGameObject)
                _lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            else if (_lastSelectedGameObject && _lastSelectedGameObject.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(_lastSelectedGameObject);
        }

        private void HandleEscapeNavigation()
        {
            if (_currentState == UIState.Gameplay)
                OpenPauseMenu();
            else if (_currentState == UIState.Paused)
                ResumeGame();
            else if (_currentState == UIState.Confirmation)
                _confirmationMenu.CallCancel();
        }

        private void OnDestroy()
            => UnsubscribeFromEvents();

        private void SubscribeToEvents()
        {
            _mainMenu.OnPlayClicked += HandlePlayClicked;
            _mainMenu.OnExitClicked += RequestExitFromApp;

            _pauseMenu.OnContinueClicked += ResumeGame;
            _pauseMenu.OnMainMenuClicked += RequestExitToMainMenu;

            if (_loreWindow != null)
                _loreWindow.OnContinueClicked += CloseLoreAndStartGame;

            if (_deathWindow != null)
            {
                _deathWindow.OnMainMenuClicked += ReloadSceneToMainMenu;
                _deathWindow.OnRespawnClicked += ReloadSceneAndStartGame;
            }

            if (_playerHealth != null)
                _playerHealth.OnDie += ShowDeathScreen;
        }

        private void UnsubscribeFromEvents()
        {
            _mainMenu.OnPlayClicked -= HandlePlayClicked;
            _mainMenu.OnExitClicked -= RequestExitFromApp;

            _pauseMenu.OnContinueClicked -= ResumeGame;
            _pauseMenu.OnMainMenuClicked -= RequestExitToMainMenu;

            if (_loreWindow != null)
                _loreWindow.OnContinueClicked -= CloseLoreAndStartGame;

            if (_deathWindow != null)
            {
                _deathWindow.OnMainMenuClicked -= ReloadSceneToMainMenu;
                _deathWindow.OnRespawnClicked -= ReloadSceneAndStartGame;
            }

            if (_playerHealth != null)
                _playerHealth.OnDie -= ShowDeathScreen;
        }

        private void CloseAllWindows()
        {
            _mainMenu.Hide();
            _gameplayUI.Hide();
            _pauseMenu.Hide();
            _confirmationMenu.Hide();
            if (_loreWindow != null) _loreWindow.Hide();
            if (_deathWindow != null) _deathWindow.Hide();
        }

        private void OpenMainMenu()
        {
            _previousState = _currentState;
            _currentState = UIState.MainMenu;
            CloseAllWindows();
            Time.timeScale = 1f;
            _mainMenu.Show();
        }

        private void HandlePlayClicked()
        {
            if (!_loreShownThisSession)
            {
                OpenLoreScreen();
            }
            else
            {
                StartGame();
            }
        }

        private void OpenLoreScreen()
        {
            _previousState = _currentState;
            _currentState = UIState.Lore;
            CloseAllWindows();
            if (_loreWindow != null) _loreWindow.Show();
        }

        private void CloseLoreAndStartGame()
        {
            _loreShownThisSession = true;
            StartGame();
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
            _backgroundMusic.Play();
        }

        private void OpenPauseMenu()
        {
            _previousState = _currentState;
            _currentState = UIState.Paused;
            Time.timeScale = 0f;
            if (_Player)
                _Player.gameObject.SetActive(false);
            _pauseMenu.Show();
            _backgroundMusic.SetVolume(_backgroundMusic.PauseVolume);
        }

        private void ResumeGame()
        {
            _previousState = _currentState;
            _currentState = UIState.Gameplay;
            Time.timeScale = 1f;
            if (_Player)
                EnablePlayer();
            _pauseMenu.Hide();
            _backgroundMusic.SetVolume(_backgroundMusic.MainVolume);
        }

        private void RequestExitToMainMenu()
        {
            _previousState = _currentState;
            _currentState = UIState.Confirmation;
            _confirmationMenu.Show(
                onConfirm: ReloadSceneToMainMenu,
                onCancel: RestoreStateAfterConfirmation,
                background: _mainMenuExitBackground);
            _backgroundMusic.Stop();
        }

        private void RequestExitFromApp()
        {
            _previousState = _currentState;
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
            _currentState = _previousState;
            if (_currentState == UIState.MainMenu) _mainMenu.Show();
            if (_currentState == UIState.Paused) _pauseMenu.Show();
        }

        private void ShowDeathScreen()
        {
            _previousState = _currentState;
            _currentState = UIState.Death;
            CloseAllWindows();
            
            if (_deathWindow != null) 
                _deathWindow.Show();
                
            if (_backgroundMusic != null) 
                _backgroundMusic.Stop();
        }

        private void ReloadSceneToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ReloadSceneAndStartGame()
        {
            Time.timeScale = 1f;
            _shouldAutoStartGame = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void EnablePlayer()
        {
            _Player.gameObject.SetActive(true);
        }
    }
}