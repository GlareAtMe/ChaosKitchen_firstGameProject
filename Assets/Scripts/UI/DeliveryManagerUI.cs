using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeTemplate;
    [SerializeField] private Transform container;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeComplited += DeliveryManager_OnRecipeComplited; ;
        DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn; 

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawn(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeComplited(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == recipeTemplate)  continue; 
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) { 
            Transform recipeTransform =  Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);

        }
    }
}
