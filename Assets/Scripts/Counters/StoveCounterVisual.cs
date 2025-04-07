using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGabeObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start() {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e) {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveOnGabeObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
