using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;

    public event EventHandler OnRestartGame;

    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

        Hide();
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            Show();

            recipesDeliveredText.text = DeliveryManager.Instance.GetAmountOfSuccessRecepes().ToString();
            playAgainButton.onClick.AddListener(() => {
                OnRestartGame?.Invoke(this, EventArgs.Empty);
            });
        } else {
            Hide();
        }
    }

    private void Update() {
        
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.gameObject.SetActive(true);
    }
}
