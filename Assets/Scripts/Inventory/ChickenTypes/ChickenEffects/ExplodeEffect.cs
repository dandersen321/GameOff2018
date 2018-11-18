using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : ChickenEffect {

    public float radius = 10.0F;
    public float power = 3000.0F;


    public ExplodeEffect(ChickenType chickenType): base(chickenType)
    {

    }

    public override void doEffect(Vector3 positionHit, GameObject objectHit)
    {
        Debug.Log("Explosive Effect!!!");
        Vector3 explosionPos = positionHit;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            Enemy enemy = hit.GetComponent<Enemy>();

            //Debug.Log("hit " + hit.name);

            if(enemy)
            {
                enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
                enemy.GetComponent<AgentMovementController>().stopMoving = false;
            }

            if (rb == null)
                continue;

            //Debug.Log("doing explosive to " + hit.name);
            rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }

        //GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //a.transform.position = explosionPos;
    }

}
