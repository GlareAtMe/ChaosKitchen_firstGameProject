using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class StoveWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;


    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnProgressAmount;

        if (show) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
