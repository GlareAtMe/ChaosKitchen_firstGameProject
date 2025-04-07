using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjects;
    public event EventHandler OnPlayerGrabObject;


    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {

            KitchenObject.SpawnKitchenObjectSO(kitchenObjects, player);
            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
