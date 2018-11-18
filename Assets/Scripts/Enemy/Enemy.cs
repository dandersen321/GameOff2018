using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Health health;
    private AttackManager attackController;
    private BodyController bodyController;
    private AgentMovementController agentController;

    public bool alive = true;

    // Use this for initialization
    void Start () {
        agentController = GetComponent<AgentMovementController>();
        health = GetComponent<Health>();
        health.onDeathAction += die;


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void die()
    {
        Debug.Log("Death!");
        alive = false;
        agentController.ragDoll();
    }
}
