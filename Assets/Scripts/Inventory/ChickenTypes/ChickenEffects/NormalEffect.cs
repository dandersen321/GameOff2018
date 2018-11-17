using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEffect : ChickenEffect {

    public NormalEffect(ChickenType chickenType): base(chickenType)
    {

    }

    public override void doEffect(Vector3 positionHit, GameObject objectHit)
    {
        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;
        enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
    }

}
