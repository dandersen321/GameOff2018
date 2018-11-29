using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedShopActivator : Item
{

    public GameObject seedShopUI;
    private DialogActivator dialogActivator;

    public void Start()
    {
        dialogActivator = GetComponent<DialogActivator>();
    }

    public override void use()
    {
        if (dialogActivator == null || (!dialogActivator.ActivateDialog() && !DialogSystem.Instance.IsActive))
            showSeedShop();
    }

    public void showSeedShop()
    {
        References.GetPlayerMovementController().beginShopMenu();
        seedShopUI.SetActive(true);
        References.getChickenUIManager().updatePlayerMoney();
    }

    public void hideSeedShop()
    {
        References.GetPlayerMovementController().closeShopMenu();
        seedShopUI.SetActive(false);
    }
}

