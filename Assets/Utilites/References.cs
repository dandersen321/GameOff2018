using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References  {

    //// Use this for initialization

    public static GameObject GetPlayer()
    {
        return GameObject.Find("Player");
    }

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

    public static Artifact getArtifact()
    {
        return GameObject.Find("Artifact").GetComponent<Artifact>();
    }

    public static ChickenUIManager getChickenUIManager()
    {
        return GameObject.Find("HUD").GetComponent<ChickenUIManager>();
    }

}
