using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChanged;

    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BuringRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BuringRecipeSO burningRecipeSO;
    private float burningTimer;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        // fried
                        GetKitchenObject().DestroyObject();
                        KitchenObject.SpawnKitchenObjectSO(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjestSO());

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                            state = state,
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        // burned
                        GetKitchenObject().DestroyObject();
                        KitchenObject.SpawnKitchenObjectSO(burningRecipeSO.output, this);
                        state = State.Burned;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                            state = state,
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // На плиті нічого немає
            if (player.HasKitchenObject()) {
                // Гравець щось тримає
                KitchenObjectSO inputKitchenObjectSO = player.GetKitchenObject().GetKitchenObjestSO();

                if (HasRecipeWithInput(inputKitchenObjectSO)) {
                    // Це можна смажити
                    player.GetKitchenObject().SetKetchenObjectParrent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

                    if (fryingRecipeSO == null) {
                        Debug.LogWarning("FryingRecipeSO is null, хоча HasRecipeWithInput повернуло true.");
                        return;
                    }

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                        state = state,
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        } else {
            // На плиті вже є інгредієнт
            if (player.HasKitchenObject()) {
                // Гравець щось тримає
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Гравець тримає тарілку
                    if (plateKitchenObject.ThyAddIngredient(GetKitchenObject().GetKitchenObjestSO())) {
                        // Успішно додано — видаляємо об’єкт і обнуляємо стан
                        GetKitchenObject().DestroyObject();

                        state = State.Idle;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                            state = state
                        });
                    }
                    // ❌ Якщо не додалось — нічого не робимо (стан не змінюється)
                }
            } else {
                // Гравець нічого не тримає — забирає об’єкт з плити
                GetKitchenObject().SetKetchenObjectParrent(player);

                state = State.Idle;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                    progressNormalized = 0f
                });

                OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                    state = state
                });
            }
        }
    }


    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);

        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO) {
        return GetFryingRecipeSOWithInput(kitchenObjectSO) != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BuringRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried() {
        return state == State.Fried;
    }
}
