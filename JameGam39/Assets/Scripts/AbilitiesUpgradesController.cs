using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitiesUpgradesController : MonoBehaviour
{

    public List<GameObject> abilities = new List<GameObject>();
    public List<GameObject> upgrades = new List<GameObject>();
    private List<GameObject> selectedPrefabs = new List<GameObject>();
    private List<GameObject> allPrefabs = new List<GameObject>();
    private bool alreadySelected;

    void Start()
    {
        allPrefabs.AddRange(abilities);
        allPrefabs.AddRange(upgrades);
        alreadySelected = true;
    }

    public void RandomUpgrades()
    {
        for(int i = 0; i < 3; i++){ // 3 random upgrades
            while(alreadySelected){
                int index = UnityEngine.Random.Range(0, allPrefabs.Count); //selects random upgrade
                if(selectedPrefabs.Contains(allPrefabs[index])){ //checks if the selected prefabs already has the random upgrade in it
                    alreadySelected = true;
                }
                else{
                    selectedPrefabs.Add(allPrefabs[index]); //adds the item that was randomly generated into selected prefabs
                    alreadySelected = false;
                }
            }
        }
    }

    public void displayUpgrades(){
        foreach (GameObject obj in selectedPrefabs){
            Instantiate(obj, new Vector2(0,0), quaternion.identity);
        }
    }
}
