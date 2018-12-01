using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtPatch : Item {

    int stage;

    public ChickenType growingChickenFood = null;

    GameObject plantObj;

    public Vector3 centerLocation;
    public bool gettingPicked = false;

    private void Start()
    {
        centerLocation =  this.GetComponent<BoxCollider>().bounds.center;
        
    }

    //private void pickPlant()
    //{
    //    References.GetPlayerMovementController().animator.SetTrigger("pickupPlant");
    //    //growingChickenFood.plantCount += 1;

    //}

    public void finishPickPlan()
    {
        growingChickenFood.chickenCount += 1;
        References.getChickenUIManager().updateDayTimeChickenCount(growingChickenFood);
        growingChickenFood = null;
        gettingPicked = true;
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

    public override bool isUsable(ChickenType chickenInHand)
    {
        if (chickenInHand == null)
            return false;
        if (chickenInHand.name == ChickenTypeEnum.normalName)
        {
            return isPickable();
        }
        
        if (growingChickenFood == null && chickenInHand.seedCount > 0)
        {
            // isPlantable
            return true;
        }

        return false;

    }
    public bool isPickable()
    {
        if (gettingPicked)
            return false;
        return growingChickenFood != null && growingChickenFood.seedStages.Count - 1 == stage;
    }

    private void plantPlant(ChickenType chickenType)
    {
        References.GetPlayerMovementController().animator.SetTrigger("plant");
        growingChickenFood = chickenType;
        growingChickenFood.seedCount -= 1;
        stage = 0;

        References.getChickenUIManager().updateDayTimeSeedCount(chickenType);
        updateStage();

        if(chickenType.seedCount <=0)
        {
            References.GetPlayerMovementController().deselectChickenSeed();
            References.getChickenUIManager().deselectChickenSlotFromItemUse(chickenType);
        }

        
        
        
    }

    public override void use()
    {
        if (growingChickenFood != null)
        {
            if(growingChickenFood.seedStages.Count -1 == stage)
            {
                gettingPicked = true;
                References.GetPlayerMovementController().animator.SetTrigger("pickupPlant");
                GameObject chickenObj = GameObject.Instantiate(References.GetPlayerMovementController().farmChickenPrefab);
                StartCoroutine(chickenObj.GetComponent<FarmChicken>().eatPlant(this));
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
