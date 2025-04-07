using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private SoundManager soundManager;

    private Animator animator;
    private int previousCountdownNumber;
    private const string NUMBER_POPUP = "OnNumberPopup"; 

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

        Hide();
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountdownStartActive()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Update() {
        int countdownNomber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNomber.ToString();

        if (previousCountdownNumber != countdownNomber) {
            previousCountdownNumber = countdownNomber;
            animator.SetTrigger(NUMBER_POPUP);
            soundManager.PlayCountdownSound();
        }
    }
    private void Hide() { 
        gameObject.SetActive(false);
    }

    private void Show() { 
        gameObject.gameObject.SetActive(true);
    }

}
