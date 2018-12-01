using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {


    //public ChickenType chickenTypes;
    //public Dictionary<ChickenType, int> chickenInventories;
    public List<ChickenType> chickenInventories;
    public List<SeedStages> seedStages;

    public GameObject chickenEquippedUI;

	// Use this for initialization
	void Start () {

        resetChickenInventories();

    }

    void resetChickenInventories()
    {
        foreach (ChickenType chickenType in chickenInventories)
        {
            chickenType.chickenCount = 15;
            chickenType.seedCount = 0;
            chickenType.currentRank = 2;
            //if (chickenType.name == ChickenTypeEnum.explosiveName)
            //{
            //   chickenType.
            //}
            //else if (chickenType.name == ChickenTypeEnum.normalName)
            //{

            //}
            //else if (chickenType.name == ChickenTypeEnum.slowName)
            //{
            //}
            //else if (chickenType.name == ChickenTypeEnum.radiationName)
            //{
            //}
            //else if (chickenType.name == ChickenTypeEnum.steroidName)
            //{
            //}
            //else if (chickenType.name == ChickenTypeEnum.heatSeekingName)
            //{
            //}
            //else
            //{
            //    // TODO remove this?
            //    throw new System.Exception("Unknown chicken? " + chickenType.ToString());
            //}
        }

        for(int i = 1; i < seedStages.Count; ++i)
        {
            chickenInventories[i].startSeed = seedStages[i].startSeed;
            chickenInventories[i].grownSeed = seedStages[i].grownPlant;
            chickenInventories[i].seedStages = seedStages[i].seedStagesPrefabs;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}


    public ChickenType getChickenKeyPressed()
    {
        for (int i = 1; i <= chickenInventories.Count; ++i)
        {
            if (Input.GetKeyDown("" + i))
            {
                return chickenInventories[i-1];
            }
        }
        return null;
    }
}
