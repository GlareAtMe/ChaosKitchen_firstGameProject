using System;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParrent
{
    public static event EventHandler OnPlasedKitchenObject;

    public static void ResetStaticData() => OnPlasedKitchenObject = null;

    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObj;

    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interack()");
    }

    public virtual void InteractAlternate(Player player) {
        //Debug.LogError("BaseCounter.InteractAlternate()");
    }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObj) {
        this.kitchenObj = kitchenObj;

        if (kitchenObj != null) {
            OnPlasedKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObj;
    }

    public void ClearKitchenObject() {
        kitchenObj = null;
    }

    public bool HasKitchenObject() {
        return kitchenObj != null;
    }

    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }
}
