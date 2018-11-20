using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopSlot : MonoBehaviour {

    Button button;
    Text costText;
    Image upgradeIcon;
    int chickenSlotIndex;
    int rankSlotIndex;
    int rank;
    ChickenType chickenType;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        costText = GetComponentInChildren<Text>();
        button.onClick.AddListener(onClick);
        chickenSlotIndex = this.gameObject.name[this.gameObject.name.Length - 3] - '0';
        rank = this.gameObject.name[this.gameObject.name.Length - 1] - '0' + 1;
        rankSlotIndex = rank == 2 ? chickenSlotIndex : chickenSlotIndex + 6;

        chickenType = References.getInventoryManager().chickenInventories[chickenSlotIndex];
        costText.text = getPrice().ToString();
        upgradeIcon = this.transform.Find("Icon").GetComponentInChildren<Image>();
        upgradeIcon.sprite = chickenType.seedSprite; // TOOD change to rank sprites
        //seedIcon.enabled = false;
    }
    
    public void onClick()
    {
        References.getChickenUIManager().buyRank(rankSlotIndex);
    }

    public int getPrice()
    {
        return chickenType.rankCosts[rank-2];
    }

    public void updateBuyable(int playerMoney)
    {
        if(playerMoney >= getPrice())
        {
            costText.color = Color.black;
        }
        else
        {
            costText.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
