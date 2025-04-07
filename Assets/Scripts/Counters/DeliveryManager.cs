using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    
    
    [SerializeField] private bool test;

    [SerializeField] private ListOfRecipeSO listOfRecipeSO;

    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeComplited;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    private float spawnTime = 5f; // Час між спавнами
    private int maxRecipes = 3; // Максимальна кількість рецептів
    private float spawnRecipeTimer;
    private int successfulRecipesAmount;


    private List<RecipeSO> waitingRecipeSOList;
    private Coroutine spawnCoroutine;

    private void Awake() {
        waitingRecipeSOList = new List<RecipeSO>();
        Instance = this;
    }

    private void Start() {
        if (test) {
            StartSpawningRecipes();
        } 

    }

    private void Update() {
        if (!test) {
            spawnRecipeTimer -= Time.deltaTime;
            if (spawnRecipeTimer <= 0f) {
                spawnRecipeTimer = spawnTime;
                if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < maxRecipes) {
                    RecipeSO waitingRecipeSO = listOfRecipeSO.recipeListSO[UnityEngine.Random.Range(0, listOfRecipeSO.recipeListSO.Count)];
                    waitingRecipeSOList.Add(waitingRecipeSO);

                    OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                //has the same number of ingredients 
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    //cycling through all ingredients in the Recipe 
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        //cycling through all ingredients in the  Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            //ingredient matches
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        //This Recipe ingredient was not found on a Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe) {
                    //player deliver correct Recipe
                    Debug.Log("player deliver correct Recipe");

                    waitingRecipeSOList.RemoveAt(i);

                    successfulRecipesAmount++;

                    OnRecipeComplited?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    
                    return;
                }
            }
        }
        //not matches found 
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        Debug.Log("player deliver incorrect Recipe");
    }




    private void StartSpawningRecipes() {
        if (spawnCoroutine == null) {
            spawnCoroutine = StartCoroutine(RecipeSpawner());
        }
    }

    private IEnumerator RecipeSpawner() {
        while (true) {
           
            while (waitingRecipeSOList.Count >= maxRecipes) {
                yield return null; 
            }

            yield return new WaitForSeconds(spawnTime);

            AddRecipe();
        }
    }

    private void AddRecipe() {
        RecipeSO newRecipe = listOfRecipeSO.recipeListSO[UnityEngine.Random.Range(0, listOfRecipeSO.recipeListSO.Count)];
        waitingRecipeSOList.Add(newRecipe);
        Debug.Log("Додано новий рецепт: " + newRecipe.name);
    }


    public List<RecipeSO> GetWaitingRecipeSOList() { 
        return waitingRecipeSOList;
    }

    public int GetAmountOfSuccessRecepes() { 
        return successfulRecipesAmount;
    }
}
