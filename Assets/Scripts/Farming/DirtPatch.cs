using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtPatch : Item {

    int stage;

    private ChickenType growingChickenFood = null;

    GameObject plantObj;

    Vector3 centerLocation;

    private void Start()
    {
        centerLocation =  this.GetComponent<BoxCollider>().bounds.center;
        
    }

    private void pickPlant()
    {
        //growingChickenFood.plantCount += 1;
        growingChickenFood.chickenCount += 1;
        References.getChickenUIManager().updateDayTimeChickenCount(growingChickenFood);
        growingChickenFood = null;

        Destroy(plantObj);
    }

    public void endNight()
    {
        if (growingChickenFood == null || isPickable())
            return;

        Destroy(plantObj);
        stage += 1;
        updateStage();
    }

    public void updateStage()
    {
        plantObj = GameObject.Instantiate(growingChickenFood.seedStages[stage]);
        plantObj.transform.position = centerLocation;
    }

    public override bool isUsable()
    {

        if(growingChickenFood == null)
        {
            return true;
        }
        else
        {
            return isPickable();
        }

    }
    private bool isPickable()
    {
        return growingChickenFood.seedStages.Count - 1 == stage;
    }

    private void plantPlant(ChickenType chickenType)
    {
        growingChickenFood = chickenType;
        growingChickenFood.seedCount -= 1;
        stage = 0;

        References.getChickenUIManager().updateDayTimeSeedCount(chickenType);
        updateStage();
    }

    public override void use()
    {
        if (growingChickenFood != null)
        {
            if(growingChickenFood.seedStages.Count -1 == stage)
            {
                pickPlant();
            }
        }
        else
        {
            ChickenType activeChickenSeed = References.GetPlayerMovementController().farmingActiveSeed;
            if (activeChickenSeed == null)
                return;

            plantPlant(activeChickenSeed);
        }
        
    }
}
