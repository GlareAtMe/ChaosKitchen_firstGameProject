using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounnter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedClearCounter == baseCounnter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(false);
        }
    }
}
