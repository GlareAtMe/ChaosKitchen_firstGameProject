using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private GameOverUI gameOverUI;

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    private enum State { 
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GaveOver,
    }

    private State state;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 15f;
    private bool isGamePaused = false;

    private void Awake() {
        state = State.WaitingToStart;
        Instance = this;
    }

    private void Start() {
        gameInput.OnPauseAction += GameInput_OnPauseAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameOverUI.OnRestartGame += GameOverUI_OnRestartGame;
    }

    private void GameOverUI_OnRestartGame(object sender, EventArgs e) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(state == State.WaitingToStart) state = State.CountdownToStart;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        if (settingsUI.IsSettingWindowOpen()) {
            settingsUI.CloseSettings();
        } else {
            TogglePauseGame(); // або твій метод з паузою
        }
    }

    private void Update() {
        switch (state) { 
            case State.WaitingToStart: 
                
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {
                    state = State.GaveOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GaveOver:
                break;
        }
    }

    public bool IsGamePlaying() { 
        return state == State.GamePlaying;
    }

    public bool IsCountdownStartActive() {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver() {
        return state == State.GaveOver;
    }

    public float GetCountdownToStartTimer() { 
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame() { 
        isGamePaused = !isGamePaused;
        if (isGamePaused) {
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            if(!settingsUI.IsSettingWindowOpen())
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

}
