using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeShopActivator : Item
{

    public GameObject gameShopUI;

    public void Start()
    {
    }

    public override void use()
    {
        showInventoryShop();
    }

    public void showInventoryShop()
    {
        References.GetPlayerMovementController().beginMenu();
        gameShopUI.SetActive(true);
        References.getChickenUIManager().updatePlayerMoney();
    }

    public void hideUpgradeShop()
    {
        References.GetPlayerMovementController().closeMenu();
        gameShopUI.SetActive(false);
    }
}

