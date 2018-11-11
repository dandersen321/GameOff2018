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
        return GameObject.Find("Turrent").GetComponent<Turrent>();
    }

    public static InventoryManager getInventoryManager()
    {
        return GameObject.Find("Player").GetComponent<InventoryManager>();
    }

}
