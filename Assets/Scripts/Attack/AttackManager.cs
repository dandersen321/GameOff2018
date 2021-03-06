﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public List<AttackData> attackData;
    private List<Attack> attacks;
    public float meeleAttackRange;
    private float globalCooldown = 0f;
    public Timer canAttackTimer = new Timer();
    private Timer checkAttackRange = new Timer();
    public bool foundTarget = false;

	// Use this for initialization
	void Start () {
        // create attacks from the attack data
        attacks = new List<Attack>();
        foreach(var data in attackData)
        {
            attacks.Add(new Attack(data, meeleAttackRange));
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!this.GetComponent<AgentMovementController>().attackPlayer)
            return;
        
        if(!canAttackTimer.Expired() || !checkAttackRange.Expired() || !GetComponent<Enemy>().alive)
        {
            // we aren't ready to attack again yet
            return;
        }

        Attack nextAttack = GetNextAttack();
        if (foundTarget || targetInRange(nextAttack))
        {
            // we found a target in range, so lets attack it
            foundTarget = true;
            BeginAttack(nextAttack);
        }
        else
        {
            this.GetComponent<AgentMovementController>().stopMoving = false;
        }
	}

    public bool targetInRange(Attack attack)
    {
        checkAttackRange.Start(0.5f);
        Vector3 targetPosition = References.GetPlayer().transform.position;
        //Debug.Log("Range: " + Vector3.Distance(targetPosition, this.transform.position));
        return Vector3.Distance(targetPosition, this.transform.position) <= attack.attackRange;
    }

    Attack GetNextAttack()
    {
        return attacks[0];
    }

    void BeginAttack(Attack attack)
    {
        Debug.Log("Beginning Attack!");
        this.GetComponent<AgentMovementController>().stopMoving = true; // stop moving once in attack range
        attack.BeginAttack(GetComponent<Animator>());
        attack.DoDamage(References.GetPlayer().GetComponent<Health>());
        canAttackTimer.Start(attack.attackLength + globalCooldown + attack.data.postAttackCooldown);
    }


}
