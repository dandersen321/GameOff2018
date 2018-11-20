using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedShopSlot : MonoBehaviour {

    Button button;
    Text costText;
    Image seedIcon;
    int slotIndex;
    ChickenType chickenType;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        costText = GetComponentInChildren<Text>();
        button.onClick.AddListener(onClick);
        slotIndex = this.gameObject.name[this.gameObject.name.Length - 1] - '0';
        chickenType = References.getInventoryManager().chickenInventories[slotIndex];
        costText.text = chickenType.cost.ToString();
        seedIcon = this.transform.Find("Icon").GetComponentInChildren<Image>();
        seedIcon.sprite = chickenType.seedSprite;
        //seedIcon.enabled = false;
    }
    
    public void onClick()
    {
        References.getChickenUIManager().buySeed(slotIndex);
    }

    public void updateBuyable(int playerMoney)
    {
        if(playerMoney >= chickenType.cost)
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
