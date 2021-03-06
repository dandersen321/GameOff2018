﻿using System.Collections;
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
    private static UpgradeShopActivator ugradeShopActivator;
    private static GameMenu gameMenu;
    public static ToolTip activeToolTip;
    private static GameObject turrentActivator;



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

    public static UpgradeShopActivator GetUpgradeShopActivator()
    {
        if (ugradeShopActivator == null)
            ugradeShopActivator = GameObject.Find("TrenchCoatAlienObj").GetComponent<UpgradeShopActivator>();
        return ugradeShopActivator;
    }

    public static GameMenu GetGameMenu()
    {
        if (gameMenu == null)
            gameMenu = GameObject.Find("GameMenu").GetComponent<GameMenu>();
        return gameMenu;
    }

    public static ToolTip GetToolTip()
    {
        return activeToolTip;
    }

    public static GameObject GetTurrentActivator()
    {
        if (turrentActivator == null)
            turrentActivator = GameObject.Find("Turrent");
        return turrentActivator;
    }


}
