using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedShopActivator : Item
{

    public GameObject seedShopUI;

    public void Start()
    {
    }

    public override void use()
    {
        showSeedShop();
    }

    public void showSeedShop()
    {
        References.GetPlayerMovementController().beginMenu();
        seedShopUI.SetActive(true);
    }

    public void hideSeedShop()
    {
        References.GetPlayerMovementController().closeMenu();
        seedShopUI.SetActive(false);
    }
}

