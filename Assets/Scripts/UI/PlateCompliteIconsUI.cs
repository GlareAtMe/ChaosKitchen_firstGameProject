using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTamplate;
    [SerializeField] private bool test;
    private void Awake() {
        iconTamplate.gameObject.SetActive(false);
    }

    private void Start() {
        plateKitchenObject.OnAddedIngredient -= PlateKitchenObject_OnAddedIngredient;
        plateKitchenObject.OnAddedIngredient += PlateKitchenObject_OnAddedIngredient;
    }

    private void PlateKitchenObject_OnAddedIngredient(object sender, PlateKitchenObject.OnAddedIngredientEventArgs e) {
        if (!test)  UpdateVisual();  else UpdateVisual1();
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if (child == iconTamplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTamplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

    private void UpdateVisual1() {
        Debug.Log("Значення списку перед оновленням UI:");
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Debug.Log(kitchenObjectSO.name);
        }

        Debug.Log("UpdateVisual() викликано! Кількість об'єктів у списку: " + plateKitchenObject.GetKitchenObjectSOList().Count);
        // Видаляємо всі іконки (крім шаблону)
        List<GameObject> childrenToRemove = new List<GameObject>();
        foreach (Transform child in transform) {
            if (child == iconTamplate) continue;
            childrenToRemove.Add(child.gameObject);
        }
        foreach (GameObject child in childrenToRemove) {
            Destroy(child);
        }

        // Додаємо нові іконки
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTamplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
