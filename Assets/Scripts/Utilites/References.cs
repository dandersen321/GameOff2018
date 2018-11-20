using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References  {

    //// Use this for initialization

    private static GameObject player;
    private static PlayerMovementController playerMovementController;
    private static Turrent turrent;
    private static InventoryManager inventoryManager;
    private static Artifact artifact;
    private static ChickenUIManager chickenUIManager;
    private static EnemySpawnerManager enemySpawnManager;
    private static SeedShopActivator seedShopActivator;



    public static GameObject GetPlayer()
    {
        if(player == null)
            player = GameObject.Find("Player");
        return player;
    }

    public static PlayerMovementController GetPlayerMovementController()
    {
        if (playerMovementController == null)
            playerMovementController = GetPlayer().GetComponent<PlayerMovementController>();
        return playerMovementController;
    }

    public static Turrent GetTurrent()
    {
        if(turrent == null)
            turrent = GameObject.Find("PlayerGun").GetComponent<Turrent>();
        return turrent;
    }

    public static InventoryManager getInventoryManager()
    {
        if(inventoryManager == null)
            inventoryManager = GameObject.Find("Player").GetComponent<InventoryManager>();
        return inventoryManager;
    }

    public static Artifact getArtifact()
    {
        if(artifact == null)
            artifact = GameObject.Find("Artifact").GetComponent<Artifact>();
        return artifact;
    }

    public static ChickenUIManager getChickenUIManager()
    {
        if(chickenUIManager == null)
            chickenUIManager = GameObject.Find("HUD").GetComponent<ChickenUIManager>();
        return chickenUIManager;
    }

    public static EnemySpawnerManager GetEnemySpawnerManager()
    {
        if(enemySpawnManager == null)
            enemySpawnManager = GameObject.Find("Spawners").GetComponent<EnemySpawnerManager>();
        return enemySpawnManager;
    }

    public static SeedShopActivator GetSeedShopActivator()
    {
        if (seedShopActivator == null)
            seedShopActivator = GameObject.Find("ImortalHuman").GetComponent<SeedShopActivator>();
        return seedShopActivator;
    }

}
