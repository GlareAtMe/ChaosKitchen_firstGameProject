using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlateCounter plateCounter;

    private List<GameObject> listOfPlateVisualGameObject;

    private void Awake() {
        listOfPlateVisualGameObject = new List<GameObject>();
    }

    private void Start() {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e) {
        GameObject plateGameObject = listOfPlateVisualGameObject[listOfPlateVisualGameObject.Count -1];
        listOfPlateVisualGameObject.Remove(plateGameObject);  
        Destroy(plateGameObject);
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e) {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);


        float plateOffsetY = .1f;
        plateVisualTransform.transform.localPosition = new Vector3(0, plateOffsetY * listOfPlateVisualGameObject.Count, 0);

        listOfPlateVisualGameObject.Add(plateVisualTransform.gameObject);
    }
}
