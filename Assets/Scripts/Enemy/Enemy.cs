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

    private int irridationCyclesLeft = 0;
    private Timer irridationPoll = new Timer();
    private int irridationRank;
    private int raditionDamge;
    private float irridationRank3Radius = 10f;
    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        agentController = GetComponent<AgentMovementController>();
        health = GetComponent<Health>();
        health.onDeathAction += die;


    }

    // Update is called once per frame
    void Update () {
		
        if(irridationCyclesLeft > 0 && irridationPoll.Expired())
        {
            doIrridationPoll();
        }
	}

    public void die()
    {
        Debug.Log("Death!");
        alive = false;
        if(References.getArtifact().heldBy == this)
        {
            // drop it
            References.getArtifact().heldBy = null;
            References.getArtifact().transform.parent = null;
            References.getArtifact().GetComponent<Rigidbody>().isKinematic = false;
            References.getArtifact().GetComponent<Rigidbody>().useGravity = true;
        }

        if (animator != null)
            animator.SetTrigger("death");
        else
            agentController.ragDoll();
    }

    public void irridate(int rank)
    {
        irridationCyclesLeft = rank == 1 ? 10 : 15;
        raditionDamge = rank == 1 ? 1 : 2;
        irridationRank = rank;
    }

    private void doIrridationPoll()
    {
        GetComponent<Health>().TakeDamage(raditionDamge);
        if(irridationRank == 3)
        {
            Vector3 explosionPos = this.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, irridationRank3Radius);

            foreach (Collider hit in colliders)
            {
                
                Enemy enemy = hit.gameObject.GetComponent<Enemy>();
                if (!enemy)
                {
                    continue;
                }

                Debug.Log("Radiation spread to " + hit.name);

                enemy.irridate(1);
            }

            //GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //a.transform.position = explosionPos;
        }

        irridationCyclesLeft -= 1;
        irridationPoll.Start(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if(bullet != null)
        {
            Debug.Log("Hit enemy via trigger");
            bullet.hitEnemy(this);
        }
    }
}
