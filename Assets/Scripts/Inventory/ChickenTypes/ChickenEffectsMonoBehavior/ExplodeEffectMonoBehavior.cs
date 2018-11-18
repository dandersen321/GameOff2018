﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffectMonoBehavior : ChickenEffectMonoBehavior {

    public float radius = 10.0F;
    public float power = 500.0F;

    public float implosionSpeed = 1f;
    public float implosionRadius = 30.0F;
    //private Timer implosionTimer = new Timer();
    private float implosionInterval = 0.5f;
    private int numberOfImplosionTicks = 6; 

    public override void doEffect()
    {
        StartCoroutine(doEffectCoroutine());
        //GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //a.transform.position = explosionPos;
    }

    private IEnumerator doEffectCoroutine()
    {

        if(rank == 3)
        {
            //implosionTimer.Start(3f);

            for(int i = 0; i < numberOfImplosionTicks; ++i)
            {
                //Debug.Log("Implosion effect Effect!!!");
                Collider[] collidersImplosion = Physics.OverlapSphere(positionHit, implosionRadius);
                foreach (Collider hit in collidersImplosion)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb == null)
                        continue;

                    AgentMovementController agentMoveContoller = hit.GetComponent<AgentMovementController>();
                    if(agentMoveContoller)
                    {
                        agentMoveContoller.stopMoving = true;
                    }

                    float step = implosionSpeed * Time.deltaTime;

                    Debug.Log("Imploding " + hit.gameObject.name);

                    // Move our position a step closer to the target.
                    rb.MovePosition(Vector3.MoveTowards(rb.transform.position, positionHit, step));

                }
                yield return new WaitForSeconds(implosionInterval);
            }

            Debug.Log("done implosion");
        }

        Debug.Log("Explosive Effect!!!");
        Vector3 explosionPos = positionHit;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            Enemy enemy = hit.GetComponent<Enemy>();

            //Debug.Log("hit " + hit.name);

            if (enemy)
            {
                int damage = rank == 1 ? chickenType.baseDamage : Convert.ToInt32(chickenType.baseDamage * 1.5);
                enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
                enemy.GetComponent<AgentMovementController>().stopMoving = false;
            }

            if (rb == null)
                continue;

            //Debug.Log("doing explosive to " + hit.name);
            rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }

        effectDone = true;
    }

}
