using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {


    public List<ChickenType> chickenInventories;

    public GameObject chickenEquippedUI;

	// Use this for initialization
	void Start () {
        resetChickenInventories();

    }

    void resetChickenInventories()
    {
        foreach (ChickenType chickenType in chickenInventories)
        {
            chickenType.count = 0;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
