using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveFlashingProgressBarUI : MonoBehaviour
{

    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnProgressAmount;

        animator.SetBool(IS_FLASHING, show);
    }
}
