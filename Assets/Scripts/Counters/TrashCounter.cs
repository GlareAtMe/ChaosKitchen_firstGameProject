using System;
using Unity.Netcode;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData() => OnAnyObjectTrashed = null;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());

            InteractLogicServerRPC();
        } else {
            //Player not carry anything 
        }
    }

    [ClientRpc]
    private void InteractLogicClientRPC() {
        InteractLogicClientRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRPC() {
        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }

}

