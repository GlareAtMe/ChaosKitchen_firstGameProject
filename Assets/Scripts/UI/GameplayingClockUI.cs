
using UnityEngine;
using UnityEngine.UI;

public class GameplayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private void Update() {
       timerImage.fillAmount =  GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
