using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParrent kitchenObjectParent;
    private FollowTransform followTransform;

    protected virtual void Awake() {
        followTransform = GetComponent<FollowTransform>();
    }

    public static void SpawnKitchenObjectSO(KitchenObjectSO kitchenObjectSO, IKitchenObjectParrent kitchenObjectParrent) {
        KitchenGameMultiplayer.Instance.SpawnKitchenObjectSO(kitchenObjectSO, kitchenObjectParrent);
    }

    public KitchenObjectSO GetKitchenObjestSO() {
        return kitchenObjectSO;
    }

    public void SetKetchenObjectParrent(IKitchenObjectParrent kitchenObjectParrent) {
        SetKitchenObjectParentServerRPC(kitchenObjectParrent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRPC(NetworkObjectReference kitchenObjectParrentNetworkObjectReference) {
        SetKitchenObjectParentClientRPC(kitchenObjectParrentNetworkObjectReference);
    }

    [ClientRpc()]
    private void SetKitchenObjectParentClientRPC(NetworkObjectReference kitchenObjectParrentNetworkObjectReference) {
        kitchenObjectParrentNetworkObjectReference.TryGet(out NetworkObject networkObjectParrectNetworkObject);
        IKitchenObjectParrent kitchenObjectParrent = networkObjectParrectNetworkObject.GetComponent<IKitchenObjectParrent>();

        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParrent;
        if (kitchenObjectParrent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParrent already has kitchenObject");
        }
        kitchenObjectParrent.SetKitchenObject(this);

        followTransform.SetTargetTransform(kitchenObjectParrent.GetKitchenObjectFollowTransform());
    }
    

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if (this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {
            plateKitchenObject = null;
            return false;
        }

    }

    public IKitchenObjectParrent GetKitchenObjectParrent() {
        return kitchenObjectParent;
    }

    public void DestroyObject() {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }
}
