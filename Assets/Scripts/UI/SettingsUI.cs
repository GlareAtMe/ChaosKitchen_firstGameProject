using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button altInteractButton;
    [SerializeField] private Button gamepadAltInteractButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI altinteractText;
    [SerializeField] private TextMeshProUGUI gamepadAltInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private GamePauseUI gamePauseUI;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform pressToRebindKeyTransform;

    private bool isSettingsOpen = false;
    public event EventHandler OnCloseSettingsWindow;

    private void Awake() {
        gamePauseUI.OnClickSettingsButton += GamePauseUI_OnClickSettingsButton;

        backButton.onClick.AddListener(() => {
            Hide();
            IsSettingWindowOpen();
            OnCloseSettingsWindow?.Invoke(this, EventArgs.Empty);
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveUp); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveDown); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveLeft); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveRight); });
        altInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.AlterInteract); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        gamepadAltInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_AlterInteract); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
    }

    private void GamePauseUI_OnClickSettingsButton(object sender, System.EventArgs e) {
        Show();
        isSettingsOpen = true;
        moveUpButton.Select();
    }

    private void Start() {
        Hide();
        HidePressToRebindKey();
        UpdateVisual();
    }


    private void UpdateVisual() {
        moveUpText.text = gameInput.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = gameInput.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = gameInput.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = gameInput.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = gameInput.GetBindingText(GameInput.Binding.Interact);
        altinteractText.text = gameInput.GetBindingText(GameInput.Binding.AlterInteract);
        gamepadAltInteractText.text = gameInput.GetBindingText(GameInput.Binding.Gamepad_AlterInteract);
        gamepadInteractText.text = gameInput.GetBindingText(GameInput.Binding.Gamepad_Interact);
    }

    private void OnDestroy() {
        gamePauseUI.OnClickSettingsButton -= GamePauseUI_OnClickSettingsButton;
    }

    private void Show() {
        gameObject.SetActive(true);
        isSettingsOpen = true;

    }

    private void Hide() {
        gameObject.SetActive(false);
        isSettingsOpen = false;
    }

    public bool IsSettingWindowOpen() {
        return isSettingsOpen;
    }

    public void SetSettingsWindowOpen(bool isOpen) {
        isSettingsOpen = isOpen;
    }

    public void CloseSettings() {
        Hide();
        OnCloseSettingsWindow?.Invoke(this, EventArgs.Empty);
    }

    private void ShowPressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(true);
    private void HidePressToRebindKey() => pressToRebindKeyTransform.gameObject.SetActive(false);

    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        gameInput.Rebinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

}
