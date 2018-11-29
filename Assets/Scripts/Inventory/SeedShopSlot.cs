﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedShopSlot : MonoBehaviour {

    Button button;
    public Text costText;
    public Text chickenTypeText;
    Image seedIcon;
    int slotIndex;
    ChickenType chickenType;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        //costText = GetComponentInChildren<Text>();
        button.onClick.AddListener(onClick);
        slotIndex = this.gameObject.name[this.gameObject.name.Length - 1] - '0';
        chickenType = References.getInventoryManager().chickenInventories[slotIndex];
        //Debug.Log("chickenType for slot " + slotIndex.ToString() + chickenType.name.ToString());
        costText.text = chickenType.cost.ToString();
        seedIcon = this.transform.Find("Icon").GetComponentInChildren<Image>();
        seedIcon.sprite = chickenType.seedSprite;
        chickenTypeText.text = chickenType.name;
        //seedIcon.enabled = false;
    }
    
    public void onClick()
    {
        Debug.Log("Seed Shop Slot clicked " + this.gameObject.name);
        References.getChickenUIManager().buySeed(slotIndex);
    }

    public void updateBuyable(int playerMoney)
    {
        if(playerMoney >= chickenType.cost)
        {
            costText.color = Color.black;
            seedIcon.color = Color.white;
        }
        else
        {
            costText.color = Color.red;
            seedIcon.color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("u");
        //button.onClick.Invoke();
    }
}
