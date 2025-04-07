using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParrent
{
    public Transform GetKitchenObjectFollowTransform();

    public void SetKitchenObject(KitchenObject kitchenObj);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();
    public bool HasKitchenObject();
}
