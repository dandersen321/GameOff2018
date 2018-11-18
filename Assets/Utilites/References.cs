using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References  {

    //// Use this for initialization

    public static PlayerMovementController GetPlayerMovementController()
    {
        return GameObject.Find("Player").GetComponent<PlayerMovementController>();
    }

    public static Turrent GetTurrent()
    {
        return GameObject.Find("PlayerGun").GetComponent<Turrent>();
    }

    public static InventoryManager getInventoryManager()
    {
        return GameObject.Find("Player").GetComponent<InventoryManager>();
    }

    public static Beacon getBeacon()
    {
        return GameObject.Find("Beacon").GetComponent<Beacon>();
    }

    public static ChickenUIManager getChickenUIManager()
    {
        return GameObject.Find("HUD").GetComponent<ChickenUIManager>();
    }

}
