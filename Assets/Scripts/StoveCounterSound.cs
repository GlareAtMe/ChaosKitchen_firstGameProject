using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private SoundManager soundManager;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Start() {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnProgressAmount;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e) {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        if (playSound) { 
            audioSource.Play();
        } else audioSource.Pause();
    }

    private void Update() {

        if (playWarningSound) {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0) {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                soundManager.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
