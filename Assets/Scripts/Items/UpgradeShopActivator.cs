using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeShopActivator : Item
{

    public GameObject gameShopUI;
    private DialogActivator dialogActivator;

    public void Start()
    {
        dialogActivator = GetComponent<DialogActivator>();
    }

    public override void use()
    {
        if (dialogActivator == null || (!dialogActivator.ActivateDialog() && !DialogSystem.Instance.IsActive))
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

