using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChickenUIManager : MonoBehaviour {

    public GameObject turrentHUD;
    private Turrent turrent;
    private GameObject chickensEquipped;

    private ChickenSlot[] chickenSlots;
    private int selectedChickenIndex = 0;

    public Canvas turrentCanvas;
    private GameObject healthBarBG;

    private SeedShopSlot[] seedShotSlots;
    private UpgradeShopSlot[] upgradeShopSlots;
    public int playerMoney = 0;

    public Text playerMoneyText;

    private bool gameStarted = false;


	// Use this for initialization
	void Start () {
        turrentHUD = GameObject.Find("TurrentHUD");
        chickensEquipped = GameObject.Find("ChickensEquipped");
        healthBarBG = GameObject.Find("HealthBarBG");
        playerMoneyText = GameObject.Find("PlayerMoney").GetComponent<Text>();
        turrent = References.GetTurrent();
        chickenSlots = chickensEquipped. GetComponentsInChildren< ChickenSlot > ();

        seedShotSlots = References.GetSeedShopActivator().seedShopUI.GetComponentsInChildren<SeedShopSlot>();
        

        upgradeShopSlots = References.GetUpgradeShopActivator().gameShopUI.GetComponentsInChildren<UpgradeShopSlot>();

        
        

        for (int i = 0; i < 6; ++i)
        {
            chickenSlots[i].setChicken(References.getInventoryManager().chickenInventories[i]);
        }
        
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

        if(!gameStarted)
        {
            gameStarted = true;
            References.GetSeedShopActivator().hideSeedShop();
            References.GetUpgradeShopActivator().hideUpgradeShop();
            
            playerMoney = 95;
            updatePlayerMoney();
            selectChickenSlot(0);
            References.GetPlayerMovementController().farmingActiveSeed = References.getInventoryManager().chickenInventories[0];
            References.getChickenUIManager().showDayUI();
        }

        

        for (int i = 1; i<=6;++i)
        {
            if(Input.GetKeyDown("" + i))
            {
                //if (i == 1 && !References.GetPlayerMovementController().turrentMode)
                //    break;

                if (References.GetPlayerMovementController().turrentMode || 
                    (chickenSlots[i - 1].chickenType.seedCount>0 || i == 1))
                {
                    selectChickenSlot(i - 1);
                }
                else
                {
                    //deselectChickenSlot(i - 1);
                }

                
                
            }
        }

        References.GetPlayerMovementController().checkFarmingKeyPress();

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

    public void deselectChickenSlot(int chickenSlotIndex)
    {
        chickenSlots[selectedChickenIndex].deselectChicken();
    }

    public void deselectChickenSlotFromItemUse(ChickenType chickenType)
    {
        foreach (ChickenSlot chickenSlot in chickenSlots)
        {
            if (chickenType == chickenSlot.chickenType)
            {
                chickenSlot.deselectChicken();
            }
        }
    }


    public void showNightUI()
    {
        healthBarBG.SetActive(true);
        foreach (ChickenSlot chickenSlot in chickenSlots)
        {
            chickenSlot.updateChickenUICount();
        }
        chickenSlots[0].imageText.text = "Normal";
        //chickenSlots[0].setChicken(References.getInventoryManager().chickenInventories[0]);
        selectChickenSlot(0);
    } 

    public void showDayUI()
    {
        
        healthBarBG.SetActive(false);  // TODO repairs during day?
        foreach (ChickenSlot chickenSlot in chickenSlots)
        {
            chickenSlot.updateSeedCount();
            chickenSlot.updateDayTimeChickenUICount();
        }

        chickenSlots[0].imageText.text = "Gather";
        //chickenSlots[0].setChicken(References.getInventoryManager().chickenInventories[0]);

        References.GetSeedShopActivator().gameObject.GetComponent<DialogActivator>().showQuestMarkerIfApplicable();
        References.GetUpgradeShopActivator().gameObject.GetComponent<DialogActivator>().showQuestMarkerIfApplicable();

        selectChickenSlot(0);
        References.GetPlayerMovementController().farmingActiveSeed = chickenSlots[0].chickenType;
        References.GetPlayerMovementController().selectChickenSeed(chickenSlots[0].chickenType);
        References.GetTurrentActivator().GetComponent<DialogActivator>().hideQuestMarker();
        References.GetTurrentActivator().GetComponent<DialogActivator>().showQuestMarkerIfApplicable();
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

    public void buySeed(int slotIndex)
    {
        ChickenType chickenType = chickenSlots[slotIndex].chickenType;
        int numberOfChickens = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 5 : 1;

        if (playerMoney < chickenType.cost * numberOfChickens || !isBuyable(chickenType))
            return;

        playerMoney -= chickenType.cost * numberOfChickens;
        chickenType.seedCount += numberOfChickens;
        chickenSlots[slotIndex].updateSeedCount();
        updatePlayerMoney();  // yeah, really should be using delegates but oh well
    }

    public bool isBuyable(ChickenType chickenType)
    {
        int daysToGrow = chickenType.seedStages.Count - 1;
        return playerMoney >= chickenType.cost && daysToGrow <= References.GetEnemySpawnerManager().waves.Count - References.GetEnemySpawnerManager().nightNumber;
    }

    public void buyRank(int rank, int chickenIndex)
    {
        //Debug.Log(slotIndex);
        ChickenType chickenType = chickenSlots[chickenIndex].chickenType;
        UpgradeShopSlot upgradeShopSlot = getUpgradeShopSlot(chickenType, rank);
        if (!upgradeShopSlot.buyable(playerMoney))
            return;

        ParticleSystem particle = Instantiate(References.GetTurrent().onLevelUpParticle, References.GetPlayer().transform.position, Quaternion.identity) as ParticleSystem;
        Destroy(particle.gameObject, References.GetTurrent().onLevelUpParticle.main.duration);

        playerMoney -= upgradeShopSlot.getPrice();
        chickenType.currentRank += 1;
        updatePlayerMoney();  // yeah, really should be using delegates but oh well
    }

    public UpgradeShopSlot getUpgradeShopSlot(ChickenType chickenType, int rank)
    {
        foreach(UpgradeShopSlot upgradeShopSlot in upgradeShopSlots)
        {
            if(upgradeShopSlot.chickenType == chickenType && upgradeShopSlot.rank == rank)
            {
                return upgradeShopSlot;
            }
        }
        throw new System.Exception("??? no getUpgradeShotSlot???");
        return null;
    }


    public void updatePlayerMoney()
    {
        if (References.GetSeedShopActivator().seedShopUI.activeSelf)
        {
            foreach (SeedShopSlot seedShotSlot in seedShotSlots)
            {
                seedShotSlot.updateBuyable(playerMoney);
            }
        }

        if (References.GetUpgradeShopActivator().gameShopUI.activeSelf)
        {
            foreach (UpgradeShopSlot upgradeShopSlot in upgradeShopSlots)
            {
                upgradeShopSlot.updateBuyable(playerMoney);
            }
        }
        playerMoneyText.text = "Money: " + playerMoney.ToString();
    }

    public void addPlayerMoney(int money)
    {
        playerMoney += money;
        updatePlayerMoney();
    }
}
