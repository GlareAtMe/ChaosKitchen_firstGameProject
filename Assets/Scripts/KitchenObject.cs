using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParrent kitchenObjectParent;

    public static KitchenObject SpawnKitchenObjectSO(KitchenObjectSO kitchenObjectSO, IKitchenObjectParrent kitchenObjectParrent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObj = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObj.SetKetchenObjectParrent(kitchenObjectParrent);

        return kitchenObj;
    }

    public KitchenObjectSO GetKitchenObjestSO() {
        return kitchenObjectSO;
    }

    public void SetKetchenObjectParrent(IKitchenObjectParrent kitchenObjectParrent) {
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParrent;
        if (kitchenObjectParrent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParrent already has kitchenObject");
        }
        kitchenObjectParrent.SetKitchenObject(this);

        transform.parent = kitchenObjectParrent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
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
