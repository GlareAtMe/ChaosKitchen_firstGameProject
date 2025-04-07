using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnAddedIngredientEventArgs> OnAddedIngredient;
    public class OnAddedIngredientEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }


    [SerializeField] private List<KitchenObjectSO> listOfValidKitchenObj;
    private List<KitchenObjectSO> listOfKitchenObject;

    private void Awake() {
        listOfKitchenObject = new List<KitchenObjectSO>();
    }

    public bool ThyAddIngredient(KitchenObjectSO kitchenObjectSO) {

        if (!listOfKitchenObject.Contains(kitchenObjectSO) && listOfValidKitchenObj.Contains(kitchenObjectSO)) {
            //object can be added
            listOfKitchenObject.Add(kitchenObjectSO);
            OnAddedIngredient?.Invoke(this, new OnAddedIngredientEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        } else {
            //already has this type
            return false;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return listOfKitchenObject;
    }
}
