using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenUIManager : MonoBehaviour {

    public GameObject chickenEquippedUI;
    private Turrent turrent;

    private ChickenSlot[] chickenSlots;
    private int selectedChickenIndex = 0;


	// Use this for initialization
	void Start () {
        chickenEquippedUI = GameObject.Find("ChickensEquipped");
        turrent = References.GetTurrent();
        chickenSlots = GetComponentsInChildren< ChickenSlot > ();
        chickenSlots[selectedChickenIndex].setChicken(References.getInventoryManager().chickenInventories[0]);
        hideChickenSlots();
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
        chickenEquippedUI.transform.localScale = Vector3.one;
        selectChickenSlot(selectedChickenIndex);
    }

    public void hideChickenSlots()
    {
        chickenEquippedUI.transform.localScale = Vector3.zero;
        //chickenEquippedUI.SetActive(false);
    }

    public void selectChickenSlot(int chickenSlotIndex)
    {
        chickenSlots[selectedChickenIndex].deselectChicken();
        selectedChickenIndex = chickenSlotIndex;
        chickenSlots[selectedChickenIndex].selectChicken();
    } 
}
