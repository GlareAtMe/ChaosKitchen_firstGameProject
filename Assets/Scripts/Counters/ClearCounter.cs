using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjects;


    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //There is no KitchenObject here 
            if (player.HasKitchenObject()) {
                // Player is carry something 
                player.GetKitchenObject().SetKetchenObjectParrent(this);
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
                } else {
                    //player is not carring plate but something else  
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if (plateKitchenObject.ThyAddIngredient(player.GetKitchenObject().GetKitchenObjestSO())) {
                            player.GetKitchenObject().DestroyObject();
                        }
                    }
                }
            } else {
                //Player not carry anything 
                GetKitchenObject().SetKetchenObjectParrent(player);
            }
        }
    }
}
