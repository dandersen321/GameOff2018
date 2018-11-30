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
    public string enemySpeed;

    private Timer lifeTimer;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        agentController = GetComponent<AgentMovementController>();
        health = GetComponent<Health>();
        health.onDeathAction += die;

        lifeTimer = new Timer();
        lifeTimer.Start(120); //in case they get stuck

    }

    // Update is called once per frame
    void Update () {
		
        if(irridationCyclesLeft > 0 && irridationPoll.Expired())
        {
            doIrridationPoll();
        }

        if(lifeTimer.Expired() && alive)
        {
            die();
        }
	}

    public void die()
    {
        StartCoroutine(deathAnimations());
    }

    public IEnumerator deathAnimations() { 
        Debug.Log("Death!");
        AudioPlayer.Instance.PlayAudio("Impact1");
        alive = false;
        agentController.stopMoving = true;
        if (References.getArtifact().heldBy == this)
        {
            // drop it
            References.getArtifact().drop();
        }

        if (enemySpeed != "fast")
        {
            // medium and large guys
            animator.SetTrigger("death");
            yield return new WaitForSeconds(3f);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        else
        {
            agentController.ragDoll();
            yield return new WaitForSeconds(5f);
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }

    public void irridate(int rank)
    {
        irridationCyclesLeft = rank == 1 ? 10 : 15;
        raditionDamge = rank == 1 ? 10 : 15;
        irridationRank = rank;
    }

    private void doIrridationPoll()
    {
        GetComponent<Health>().TakeDamage(raditionDamge);
        ParticleSystem particle = Instantiate(References.GetTurrent().onRadiationParticle, transform.position, Quaternion.identity) as ParticleSystem;
        Destroy(particle.gameObject, References.GetTurrent().onRadiationParticle.main.duration);
        if (irridationRank == 3)
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
        if(alive && bullet != null)
        {
            Debug.Log("Hit enemy via trigger");
            bullet.hitEnemy(this.gameObject);
        }
    }
}
