using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChickenEffect {

    protected ChickenType chickenType;

    public ChickenEffect(ChickenType chickenType)
    {
        this.chickenType = chickenType;
    }

    public abstract void doEffect(Vector3 positionHit, GameObject objectHit);
}
