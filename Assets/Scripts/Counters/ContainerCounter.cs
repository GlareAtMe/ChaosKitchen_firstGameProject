using System;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjects;
    public event EventHandler OnPlayerGrabObject;


    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {

            KitchenObject.SpawnKitchenObjectSO(kitchenObjects, player);

            InteractLogicServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRPC() {
        InteractLogicClientRPC();
    }
    
    [ClientRpc]
    private void InteractLogicClientRPC() {
        OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
    }
}
