using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenUIManager : MonoBehaviour {

    public GameObject turrentHUD;
    private Turrent turrent;

    private ChickenSlot[] chickenSlots;
    private int selectedChickenIndex = 0;

    public Canvas turrentCanvas;


	// Use this for initialization
	void Start () {
        turrentHUD = GameObject.Find("TurrentHUD");
        turrent = References.GetTurrent();
        chickenSlots = GetComponentsInChildren< ChickenSlot > ();
        chickenSlots[selectedChickenIndex].setChicken(References.getInventoryManager().chickenInventories[0]);
        chickenSlots[1].setChicken(References.getInventoryManager().chickenInventories[1]);
        hideChickenSlots();
    }

    public ChickenType getActiveChicken()
    {
        return chickenSlots[selectedChickenIndex].chickenType;
    }
	
	// Update is called once per frame
	void Update () {
		if(turrent.turrentModeActive)
        {
            for(int i = 1; i<=6;++i)
            {
                if(Input.GetKeyDown("" + i))
                {
                    selectChickenSlot(i-1);
                }
            }
        }
	}

    public ChickenSlot getSelectedChickenSlot()
    {
        return chickenSlots[selectedChickenIndex];
    }

    public void showChickenSlots()
    {
        //chickenEquippedUI.SetActive(true);
        //turrentHUD.transform.localScale = Vector3.one;
        //turrentHUD.SetActive(true);
        turrentCanvas.enabled = true;
        selectChickenSlot(selectedChickenIndex);
    }

    public void hideChickenSlots()
    {
        turrentCanvas.enabled = false;
        //turrentHUD.transform.position = new Vector3(-10000, -10000, -10000);
        //turrentHUD.SetActive(false);
        //turrentHUD.transform.localScale = Vector3.zero;
        //chickenEquippedUI.SetActive(false);
    }

    public void selectChickenSlot(int chickenSlotIndex)
    {
        chickenSlots[selectedChickenIndex].deselectChicken();
        selectedChickenIndex = chickenSlotIndex;
        chickenSlots[selectedChickenIndex].selectChicken();
    } 
}
