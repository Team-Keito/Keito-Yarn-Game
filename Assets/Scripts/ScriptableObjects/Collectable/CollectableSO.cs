using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Collectable", menuName = "SO/Collectable")]
public class CollectableSO : ScriptableObject
{
    public CatFactSO CatFact;
    public bool isCollected;

    public int points = 5;


    private void OnEnable()
    {
    #if UNITY_EDITOR 
        isCollected = false;
    #endif
    }
}
