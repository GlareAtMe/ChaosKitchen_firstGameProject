using System;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 5f;
    private int spawnPlateAmount;
    private int spawnPlateAmountMax = 5;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && spawnPlateAmount < spawnPlateAmountMax) {
                spawnPlateAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            if (spawnPlateAmount > 0) {
                spawnPlateAmount--;
                KitchenObject.SpawnKitchenObjectSO(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
