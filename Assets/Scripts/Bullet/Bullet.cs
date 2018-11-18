using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    /// <summary>
    /// How long bullet will last before being destroyed
    /// </summary>
    float lifeTime = 10f;
    float timeUntilGravity = 0.25f;

    /// <summary>
    /// Keep track of when the bullet has expired
    /// </summary>
    Timer lifeTimer = new Timer();
    Timer gravityTimer = new Timer();
    private bool bulletSpent = false;

    public int damage = 100;

    public ChickenType chickenType;
    private int rank;

    public bool heatSeeking = false;
    public GameObject target;
    public Vector3 defaultTargetPosition;
    private Timer lookForTarget = new Timer();
    private float moveToTargetSpeed = 12f;

    public void init(ChickenType chickenType, int rank)
    {
        this.chickenType = chickenType;
        this.rank = rank;
    }

    void Start () {
        lifeTimer.Start(lifeTime);
        gravityTimer.Start(timeUntilGravity);
    }
	
	void Update ()
    {
        if (heatSeeking)
        {
            if (target.GetComponent<Enemy>().alive == false)
                target = null;
            if (target == null && lookForTarget.Expired())
                findTarget();

            Vector3 targetPosition = target == null ? defaultTargetPosition : target.transform.position;

            float step = moveToTargetSpeed * Time.deltaTime;

            // Move our position a step closer to the target.
            this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        }
        else
        {
            if (lifeTimer.Expired())
            {
                Destroy(this.gameObject);
            }
            if (gravityTimer.Expired())
            {
                this.GetComponent<Rigidbody>().useGravity = true;
            }
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
            return;
        //Debug.Log("Hit " + collision.gameObject.name);
        if (!bulletSpent)
        {
            bulletSpent = true;
        }

        startEffects(collision.gameObject);

        //Health health = collision.gameObject.GetComponent<Health>();
        //if(health)
        //{
        //    health.TakeDamage(damage);
        //}

        Destroy(this.gameObject);
    }

    public void findTarget()
    {
        GameObject closestTarget = null;
        float? closestSqrMagnitude = null;
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (gameObject.GetComponent<Enemy>().alive == false)
                continue;
            float sqrMagnitude = (this.transform.position - gameObject.transform.position).sqrMagnitude;

            if (closestSqrMagnitude == null || sqrMagnitude < closestSqrMagnitude)
            {
                closestTarget = gameObject;
                closestSqrMagnitude = sqrMagnitude;
            }

        }

        if (closestTarget == null) {
            lookForTarget.Start(2f);
        }
        else
        {
            target = closestTarget;
        }

    }

    void startEffects(GameObject objectHit)
    {
        GameObject effectObj = new GameObject();
        if (chickenType.name == ChickenTypeEnum.explosiveName)
        {
            effectObj.AddComponent<ExplodeEffectMonoBehavior>();
            effectObj.GetComponent<ChickenEffectMonoBehavior>().init(chickenType, this.transform.position, objectHit, rank);
        }
        else if (chickenType.name == ChickenTypeEnum.normalName)
        {
            //effectObj.AddComponent<NormalEffectMonoBehavior>();
            doNormalEffect(objectHit);
        }
        else if (chickenType.name == ChickenTypeEnum.slowName)
        {

        }
        else if(chickenType.name == ChickenTypeEnum.radiationName)
        {

        }
        else if(chickenType.name == ChickenTypeEnum.steroidName)
        {
            doSteriodEffect(objectHit);
        }
        else if(chickenType.name == ChickenTypeEnum.heatSeekingName)
        {

        }
        else
        {
            // TODO remove this?
            throw new System.Exception("Unknown chicken? " + chickenType.ToString());
        }


        
    }

    void doNormalEffect(GameObject objectHit)
    {
        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;
        enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
    }

    void doSteriodEffect(GameObject objectHit)
    {
        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;
        enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);

        if(rank==3)
        {

        }
    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    Debug.Log("Hit " + collider.name);
    //    if (collider.tag != "TriggerConsumesBullet")
    //        return;

    //    if (!bulletSpent)
    //    {
    //        bulletSpent = true;
    //    }
    //    Destroy(this.gameObject);
    //}
}
