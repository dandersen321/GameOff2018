using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChickenEffectMonoBehavior: MonoBehaviour
{

    protected bool effectStarted = false;
    protected ChickenType chickenType;
    protected Vector3 positionHit;
    protected GameObject objectHit;
    protected int rank;
    protected bool effectDone = false;

    public void init(ChickenType chickenType, Vector3 positionHit, GameObject objectHit, int rank)
    {
        this.chickenType = chickenType;
        this.positionHit = positionHit;
        this.objectHit = objectHit;
        this.rank = rank;
    }

    public void Update()
    {
        if (!effectStarted)
            doEffect();

        if (effectDone)
        {
            Debug.Log("Effect Done!");
            Destroy(this.gameObject);
        }
    }

    public abstract void doEffect();
}
