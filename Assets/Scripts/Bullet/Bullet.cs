using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    /// <summary>
    /// How long bullet will last before being destroyed
    /// </summary>
    float lifeTime = 15f;
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

    public ParticleSystem onHitParticle;
    public ParticleSystem onHitEnemyParticle;

    public void init(ChickenType chickenType)
    {
        GetComponent<Animator>().SetTrigger("attack");
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

            if (gravityTimer.Expired())
            {
                this.GetComponent<Rigidbody>().useGravity = true;
            }
        }

        if (lifeTimer.Expired())
        {
            if(target != null && target.GetComponent<Enemy>() != null && heatSeeking)
            {
                target.GetComponent<Enemy>().targetedByMissle = false;
            }
            Debug.Log("Bullet expired");
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.GetComponent<Bullet>() != null)
            return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (chickenType.spentOnNonEnemyImpact || enemy)
        {
            hitObject(collision.gameObject);
        }

      

        
    }

    public void hitObject(GameObject objectHit)
    {
        startEffects(objectHit);
        if (onHitParticle != null)
        {
            AudioPlayer.Instance.PlayAudio("Impact1");
            Debug.Log("Spawning particles");
            ParticleSystem systemToUse;
            if(objectHit.GetComponent<Enemy>() != null)
            {
                systemToUse = onHitEnemyParticle;
            }
            else
            {
                systemToUse = onHitParticle;
            }
            ParticleSystem particle = Instantiate(systemToUse, transform.position, Quaternion.identity) as ParticleSystem;
            Destroy(particle.gameObject, systemToUse.main.duration);
        }
        else
            Debug.Log("Not spawning particles");


        if (chickenType.name != ChickenTypeEnum.slowName || chickenType.currentRank < 3)
        {
            if (objectHit.GetComponent<Enemy>() == null && heatSeeking)
            {
                return;
            }
            else
            {
                if(heatSeeking && target != null && target.GetComponent<Enemy>() != null)
                {
                    target.GetComponent<Enemy>().targetedByMissle = false;
                }
                Destroy(this.gameObject);
            }
        }
    }

    public void spawnMiniMissiles()
    {
        BulletFactory bulletFactory = new BulletFactory();
        GameObject chickenPrefab = References.GetTurrent().storedBulletObject;

        Vector2 offsetV2 = (Random.insideUnitCircle.normalized * 25f);
        Vector3 targetPosition = this.transform.position + new Vector3(offsetV2.x, 0, offsetV2.y);

        GameObject bulletObject = bulletFactory.createBullet(chickenPrefab, this.transform.position, targetPosition, chickenType, true, References.GetTurrent().onHitParticle, References.GetTurrent().onHitEnemyParticle);
        heatSeekingSpawner.Start(2f);
    }

    public void findTarget()
    {
        if (chickenType.currentRank == 3 && !heatSeekingMini)
            return;

        if (References.getArtifact().heldBy != null)
        {
            target = References.getArtifact().heldBy.gameObject;
            return;
        }




        GameObject closestTarget = null;
        float? closestSqrMagnitude = null;
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (gameObject.GetComponent<Enemy>().alive == false)
                continue;
            if(gameObject.GetComponent<Enemy>().targetedByMissle == true)
            {
                Debug.Log("Ignoring enemey because alreayd targeted");
                continue;
            }
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
            Debug.Log("Found target " + closestTarget.gameObject.name);
            target = closestTarget;
            target.GetComponent<Enemy>().targetedByMissle = true;
            
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

        blowEnemyIfApplicable(objectHit);



    }

    void blowEnemyIfApplicable(GameObject objectHit)
    {
        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (!enemy || enemy.enemySpeed != "fast")
            return;

        Vector3 direction = objectHit.transform.position - transform.position;
        direction.Normalize();
        enemy.gameObject.GetComponent<Rigidbody>().AddForce(direction * 1000f);

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
        Debug.Log("did damge to " + objectHit.name);
        enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
    }

    void doSteriodEffect(GameObject objectHit)
    {

        if (chickenType.currentRank == 3 && !heatSeekingMini)
        {
            Debug.Log("steroid rank 3 effect");
            for (int i = 0; i < 3; ++i)
            {
                spawnMiniMissiles();
            }
            //BulletFactory bulletFactory = new BulletFactory();
            //GameObject chickenPrefab = References.GetTurrent().storedBulletObject;
            //ChickenType normalChicken = References.getInventoryManager().chickenInventories[0];
            //float chickenRange = 10;
            //for (int i = 0; i < 5; ++i)
            //{
            //    //Debug.Log("Creating " + i);

            //    Vector2 offsetV2 = (Random.insideUnitCircle.normalized * chickenRange);
            //    Vector3 targetPosition = this.transform.position + new Vector3(offsetV2.x, 0, offsetV2.y);


            //    bulletFactory.createBullet(chickenPrefab, this.transform.position, targetPosition, normalChicken);
            //}
        }

        Enemy enemy = objectHit.GetComponent<Enemy>();
        if (enemy == null)
            return;
        int rankModifer = chickenType.currentRank == 1 ? 1 : 2;
        enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage * rankModifer);
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
        

        float slowDownModifer = chickenType.currentRank == 1 ? 0.65f : 0f;
        float slowDownLength = 3f;
        float radius = 7f;

        if (chickenType.currentRank == 3)
        {

            Vector3 explosionPos = objectHit.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

            foreach (Collider hit in colliders)
            {
                //Rigidbody rb = hit.GetComponent<Rigidbody>();
                Enemy enemy = hit.GetComponent<Enemy>();

                //Debug.Log("hit " + hit.name);

                if (enemy)
                {
                    //int damage = chickenType.currentRank == 1 ? chickenType.baseDamage : System.Convert.ToInt32(chickenType.baseDamage * 1.5);
                    //enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
                    //enemy.GetComponent<AgentMovementController>().stopMoving = false;
                    enemy.GetComponent<AgentMovementController>().slowDown(slowDownModifer, slowDownLength);
                }

                //if (rb == null)
                //    continue;

                //Debug.Log("doing explosive to " + hit.name);
                //rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
        }
        else
        {
            Enemy enemy = objectHit.GetComponent<Enemy>();
            if (enemy == null)
                return;

            enemy.GetComponent<AgentMovementController>().slowDown(slowDownModifer, slowDownLength);
            enemy.GetComponent<Health>().TakeDamage(chickenType.baseDamage);
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
