using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnAlternatInteractionAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnRebindingBind;


    private PlayerInputActions playerInputActions;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        AlterInteract,
        Gamepad_Interact,
        Gamepad_AlterInteract,
    }


    private void Awake() {
        playerInputActions = new PlayerInputActions();
        //need do this after construct object and before enable actionMap
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.AlternativeAction.performed += AlternativeAction_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;


    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.AlternativeAction.performed -= AlternativeAction_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void AlternativeAction_performed(InputAction.CallbackContext obj) {
        OnAlternatInteractionAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.AlterInteract:
                return playerInputActions.Player.AlternativeAction.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_AlterInteract:
                return playerInputActions.Player.AlternativeAction.bindings[1].ToDisplayString();
            case Binding.MoveUp:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
        }
    }

    public void Rebinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.MoveUp:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.AlterInteract:
                inputAction = playerInputActions.Player.AlternativeAction;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_AlterInteract:
                inputAction = playerInputActions.Player.AlternativeAction;
                bindingIndex = 1;
                break;

        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
                  .OnComplete(callback => {
                      callback.Dispose();
                      playerInputActions.Player.Enable();
                      onActionRebound();

                      PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                      PlayerPrefs.Save();

                      OnRebindingBind?.Invoke(this, EventArgs.Empty);
                  })
                  .Start();
    }

}
