using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI InteractText;
    [SerializeField] private TextMeshProUGUI AlterInteractText;
    [SerializeField] private TextMeshProUGUI GamepadInteractText;
    [SerializeField] private TextMeshProUGUI GamepadAlterInteractText;

    [SerializeField]private GameInput gameInput;

    private void Start() {
        gameInput.OnRebindingBind += GameInput_OnRebindingBind;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;

        UpdateVisual();

        Show();
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountdownStartActive()) Hide(); 
    }

    private void GameInput_OnRebindingBind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        moveUpText.text = gameInput.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = gameInput.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = gameInput.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = gameInput.GetBindingText(GameInput.Binding.MoveRight);
        InteractText.text = gameInput.GetBindingText(GameInput.Binding.Interact);
        AlterInteractText.text = gameInput.GetBindingText(GameInput.Binding.AlterInteract);
        GamepadInteractText.text = gameInput.GetBindingText(GameInput.Binding.Gamepad_Interact);
        GamepadAlterInteractText.text = gameInput.GetBindingText(GameInput.Binding.Gamepad_AlterInteract);
    }

    private void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);
}
