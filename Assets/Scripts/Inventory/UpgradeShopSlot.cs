using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopSlot : MonoBehaviour {

    Button button;
    Text costText;
    Image upgradeIcon;
    int chickenSlotIndex;
    //int rankSlotIndex;
    public int rank;
    public ChickenType chickenType;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        costText = GetComponentInChildren<Text>();
        button.onClick.AddListener(onClick);
        chickenSlotIndex = this.gameObject.name[this.gameObject.name.Length - 3] - '0';
        rank = this.gameObject.name[this.gameObject.name.Length - 1] - '0' + 1;
        //rankSlotIndex = rank == 2 ? chickenSlotIndex : chickenSlotIndex + 5;

        chickenType = References.getInventoryManager().chickenInventories[chickenSlotIndex];
        costText.text = getPrice().ToString();
        upgradeIcon = this.transform.Find("Icon").GetComponentInChildren<Image>();
        upgradeIcon.sprite = chickenType.seedSprite; // TOOD change to rank sprites
        //seedIcon.enabled = false;
    }
    
    public void onClick()
    {
        Debug.Log("upgrade slot clicked! " + this.gameObject.name);
        References.getChickenUIManager().buyRank(rank, chickenSlotIndex);
    }

    public int getPrice()
    {
        return chickenType.rankCosts[rank-2];
    }

    public bool buyable(int playerMoney)
    {
        return playerMoney >= getPrice() && chickenType.currentRank +1 == rank;
    }

    public void updateBuyable(int playerMoney)
    {
        if(buyable(playerMoney))
        {
            costText.color = Color.black;
            upgradeIcon.color = Color.white;
        }
        else
        {
            costText.color = Color.red;
            upgradeIcon.color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
