using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData() => OnAnyCut = null;


    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChanged;
    public event EventHandler OnCut;



    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //There is no KitchenObject here 
            if (player.HasKitchenObject()) {
                // Player is carry something 
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjestSO())) {
                    player.GetKitchenObject().SetKetchenObjectParrent(this);
                    cuttingProgress = 0;
                    var cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjestSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            } else {
                //Player not carry anything 
            }
        } else {
            //There is KitchenObject here 
            if (player.HasKitchenObject()) {
                // Player is carry something 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //player holding plate
                    if (plateKitchenObject.ThyAddIngredient(GetKitchenObject().GetKitchenObjestSO())) {
                        GetKitchenObject().DestroyObject();
                    }
                }
            } else {
                //Player not carry anything 
                GetKitchenObject().SetKetchenObjectParrent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjestSO())) {
            // there is kitchen object AND it can be cut
            cuttingProgress += 1;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            var cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjestSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            }
                   );
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjestSO());
                GetKitchenObject().DestroyObject();

                KitchenObject.SpawnKitchenObjectSO(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);

        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else { return null; }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
