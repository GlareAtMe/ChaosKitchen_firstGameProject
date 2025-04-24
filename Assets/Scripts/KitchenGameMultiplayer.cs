using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{

    [SerializeField] private KitchenObjectsSOList kitchenObjectsSOList;

    public static KitchenGameMultiplayer Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void SpawnKitchenObjectSO(KitchenObjectSO kitchenObjectSO, IKitchenObjectParrent kitchenObjectParrent) {
        SpawnKitchenObjectSOServerRPC(GetKitchenObjectSOIndex(kitchenObjectSO), kitchenObjectParrent.GetNetworkObject());
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectSOServerRPC(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParrentNetworkObjectReference) {
        KitchenObjectSO kitchenObjectSO =  GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetworkObject kitchenObjecttNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjecttNetworkObject.Spawn(true);
        KitchenObject kitchenObj = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParrentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParrentNetworkObject);
        IKitchenObjectParrent kitchenObjectParrent = kitchenObjectParrentNetworkObject.GetComponent<IKitchenObjectParrent>();
        kitchenObj.SetKetchenObjectParrent(kitchenObjectParrent);
        Debug.Log(kitchenObj.transform.position);
    }

    private int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObjectSO) { 
        return kitchenObjectsSOList.KitchenObjectList.IndexOf(kitchenObjectSO);
    }

    private KitchenObjectSO GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex) {
       return kitchenObjectsSOList.KitchenObjectList[kitchenObjectSOIndex];
    }
}
