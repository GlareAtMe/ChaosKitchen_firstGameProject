using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingMenuButton;

    public event EventHandler OnClickSettingsButton;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scenes.MenuScene);
        });

        settingMenuButton.onClick.AddListener(() => {
            Hide();
            OnClickSettingsButton?.Invoke(this, EventArgs.Empty);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        settingsUI.OnCloseSettingsWindow += SettingsUI_OnCloseSettingsWindow;

        Hide();
    }

    private void SettingsUI_OnCloseSettingsWindow(object sender, EventArgs e) {
         Show();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
        if (!settingsUI.IsSettingWindowOpen()) Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
    }

    private void Show() {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide() => gameObject.SetActive(false);
}
