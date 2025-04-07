using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuringRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;   
    public KitchenObjectSO output;
    public float burningTimerMax;
}
