using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenUIManager : MonoBehaviour {

    public GameObject turrentHUD;
    private Turrent turrent;
    private GameObject chickensEquipped;

    private ChickenSlot[] chickenSlots;
    private int selectedChickenIndex = 0;

    public Canvas turrentCanvas;
    private GameObject healthBarBG;


	// Use this for initialization
	void Start () {
        turrentHUD = GameObject.Find("TurrentHUD");
        chickensEquipped = GameObject.Find("ChickensEquipped");
        healthBarBG = GameObject.Find("HealthBarBG");
        turrent = References.GetTurrent();
        chickenSlots = chickensEquipped. GetComponentsInChildren< ChickenSlot > ();
        for(int i = 0; i < 6; ++i)
        {
            chickenSlots[i].setChicken(References.getInventoryManager().chickenInventories[i]);
        }
        showDayUI();
        //chickenSlots[selectedChickenIndex].setChicken(References.getInventoryManager().chickenInventories[0]);
        //chickenSlots[1].setChicken(References.getInventoryManager().chickenInventories[1]);
        //chickenSlots[2].setChicken(References.getInventoryManager().chickenInventories[2]);
        //hideTurrentHUD();
    }

    public ChickenType getActiveChicken()
    {
        return chickenSlots[selectedChickenIndex].chickenType;
    }
	
	// Update is called once per frame
	void Update () {
        for(int i = 1; i<=6;++i)
        {
            if(Input.GetKeyDown("" + i))
            {
                selectChickenSlot(i-1);
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

    //public void hideTurrentHUD()
    //{
    //    turrentCanvas.enabled = false;
    //    //turrentHUD.transform.position = new Vector3(-10000, -10000, -10000);
    //    //turrentHUD.SetActive(false);
    //    //turrentHUD.transform.localScale = Vector3.zero;
    //    //chickenEquippedUI.SetActive(false);
    //}

    public void selectChickenSlot(int chickenSlotIndex)
    {
        chickenSlots[selectedChickenIndex].deselectChicken();
        selectedChickenIndex = chickenSlotIndex;
        chickenSlots[selectedChickenIndex].selectChicken();
    }
    
    public void showNightUI()
    {
        healthBarBG.SetActive(true);
        foreach (ChickenSlot chickenSlot in chickenSlots)
        {
            chickenSlot.updateChickenUICount();
        }
    } 

    public void showDayUI()
    {
        healthBarBG.SetActive(false);  // TODO repairs during day?
        foreach (ChickenSlot chickenSlot in chickenSlots)
        {
            chickenSlot.updateSeedCount();
            chickenSlot.updateDayTimeChickenUICount();
        }
    }

    public void updateDayTimeSeedCount(ChickenType chickenType)
    {
        foreach (ChickenSlot chickenSlot in chickenSlots)
        {
            if (chickenType == chickenSlot.chickenType)
            {
                chickenSlot.updateSeedCount();
            }
        }
    }

    public void updateDayTimeChickenCount(ChickenType chickenType)
    {
        foreach(ChickenSlot chickenSlot in chickenSlots)
        {
            if(chickenType == chickenSlot.chickenType)
            {
                chickenSlot.updateDayTimeChickenUICount();
            }
        }
    }
}
