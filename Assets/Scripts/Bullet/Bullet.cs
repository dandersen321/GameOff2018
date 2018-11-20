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

    public bool heatSeeking = false;
    public GameObject target;
    public Vector3 defaultTargetPosition;
    private Timer lookForTarget = new Timer();
    public Timer heatSeekingSpawner = new Timer();
    private float moveToTargetSpeed = 12f;
    public bool heatSeekingMini = false;

    public void init(ChickenType chickenType)
    {
        this.chickenType = chickenType;

        lifeTimer.Start(lifeTime);
        gravityTimer.Start(timeUntilGravity);
    }
	
	void Update ()
    {
        if (heatSeeking)
        {
            if (target != null && target.GetComponent<Enemy>().alive == false)
                target = null;
            if (target == null && lookForTarget.Expired())
                findTarget();
            if (chickenType.currentRank == 3 && !heatSeekingMini && heatSeekingSpawner.Expired())
                spawnMiniMissiles();

            Vector3 targetPosition = target == null ? defaultTargetPosition : target.transform.position;

            float step = moveToTargetSpeed * Time.deltaTime;

            if (chickenType.currentRank == 3 && !heatSeekingMini)
            {
                step /= 3;
            }

            // Move our position a step closer to the target.
            this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        }
        else
        {
            if (lifeTimer.Expired())
            {
                Debug.Log("Bullet expired");
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

        if (chickenType.spentOnNonEnemyImpact || collision.gameObject.GetComponent<Enemy>())
        {
            startEffects(collision.gameObject);
            Destroy(this.gameObject);
        }

        


        //Health health = collision.gameObject.GetComponent<Health>();
        //if(health)
        //{
        //    health.TakeDamage(damage);
        //}

        //if(chickenType.name == ChickenTypeEnum.slowName && chickenType.currentRank == 3)
        //if(true)
        //{
        //    // don't get spent
        //    return;
        //}

        
    }

    public void spawnMiniMissiles()
    {
        BulletFactory bulletFactory = new BulletFactory();
        GameObject chickenPrefab = References.GetTurrent().storedBulletObject;

        Vector2 offsetV2 = (Random.insideUnitCircle.normalized * 25f);
        Vector3 targetPosition = this.transform.position + new Vector3(offsetV2.x, 0, offsetV2.y);

        GameObject bulletObject = bulletFactory.createBullet(chickenPrefab, this.transform.position, targetPosition, chickenType, true);
        heatSeekingSpawner.Start(2f);
    }

    public void findTarget()
    {
        if (chickenType.currentRank == 3 && !heatSeekingMini)
            return;
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
            Debug.Log("No luck");
        }
        else
        {
            target = closestTarget;
            Debug.Log("Found target " + target.gameObject.name);
        }

    }

    void startEffects(GameObject objectHit)
    {
        if (chickenType.name == ChickenTypeEnum.explosiveName)
        {
            GameObject effectObj = new GameObject();
            effectObj.AddComponent<ExplodeEffectMonoBehavior>();
            effectObj.GetComponent<ChickenEffectMonoBehavior>().init(chickenType, this.transform.position, objectHit, chickenType.currentRank);
        }
        else if (chickenType.name == ChickenTypeEnum.normalName)
        {
            //effectObj.AddComponent<NormalEffectMonoBehavior>();
            doNormalEffect(objectHit);
        }
        else if (chickenType.name == ChickenTypeEnum.slowName)
        {
            doSlowEffect(objectHit);
        }
        else if(chickenType.name == ChickenTypeEnum.radiationName)
        {
            doRadiationEffect(objectHit);
        }
        else if(chickenType.name == ChickenTypeEnum.steroidName)
        {
            doSteriodEffect(objectHit);
        }
        else if(chickenType.name == ChickenTypeEnum.heatSeekingName)
        {
            doHeatSeekingEffect(objectHit);
        }
        else
        {
            // TODO remove this?
            throw new System.Exception("Unknown chicken? " + chickenType.ToString());
        }


        
    }

    void doRadiationEffect(GameObject objectHit)
    {
        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.irridate(chickenType.currentRank);
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

        if (chickenType.currentRank == 3)
        {
            Debug.Log("steroid rank 3 effect");
            BulletFactory bulletFactory = new BulletFactory();
            GameObject chickenPrefab = References.GetTurrent().storedBulletObject;
            ChickenType normalChicken = References.getInventoryManager().chickenInventories[0];
            float chickenRange = 10;
            for (int i = 0; i < 5; ++i)
            {
                //Debug.Log("Creating " + i);

                Vector2 offsetV2 = (Random.insideUnitCircle.normalized * chickenRange);
                Vector3 targetPosition = this.transform.position + new Vector3(offsetV2.x, 0, offsetV2.y);


                bulletFactory.createBullet(chickenPrefab, this.transform.position, targetPosition, normalChicken);
            }
        }

        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;
        enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
    }

    void doHeatSeekingEffect(GameObject objectHit)
    {
        float radius = 10.0F;
        float power = 500.0F;

        Vector3 explosionPos = objectHit.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            Enemy enemy = hit.GetComponent<Enemy>();

            //Debug.Log("hit " + hit.name);

            if (enemy)
            {
                int damage = chickenType.currentRank == 1 ? chickenType.baseDamage : System.Convert.ToInt32(chickenType.baseDamage * 1.5);
                enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
                enemy.GetComponent<AgentMovementController>().stopMoving = false;
            }

            if (rb == null)
                continue;

            //Debug.Log("doing explosive to " + hit.name);
            rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }

    void doSlowEffect(GameObject objectHit)
    {
        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;

        float slowDownModifer = chickenType.currentRank == 1 ? 0.5f : 0f;
        float slowDownLength = 3f;

        enemy.GetComponent<AgentMovementController>().slowDown(slowDownModifer, slowDownLength);
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
