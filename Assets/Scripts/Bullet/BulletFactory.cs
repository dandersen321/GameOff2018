using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory {

    public GameObject createBullet(GameObject bulletObjPrefab, Vector3 startPosition, Vector3 targetPosition, ChickenType chickenType, bool heatMiniMissle = false, ParticleSystem onHitParticle = null, ParticleSystem onHitEnemyParticle = null)
    {
        GameObject bulletObj = GameObject.Instantiate(bulletObjPrefab);
        bulletObj.GetComponentInChildren<Renderer>().material = chickenType.chickenColorMaterial;
        bulletObj.SetActive(true);
        bulletObj.transform.position = startPosition;
        bulletObj.transform.LookAt(targetPosition);

        float chickenSizeScale = chickenType.chickenSizeScale;
        

        bulletObj.AddComponent<Bullet>();
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.init(chickenType);
        bullet.onHitParticle = onHitParticle;
        bullet.onHitEnemyParticle = onHitEnemyParticle;
        var rb = bulletObj.GetComponent<Rigidbody>();

        if(chickenType.name == ChickenTypeEnum.slowName && chickenType.currentRank == 3)
        {
            rb.useGravity = false;
            rb.GetComponent<SphereCollider>().isTrigger = true;
        }

        if (chickenType.name == ChickenTypeEnum.heatSeekingName || heatMiniMissle)
        {
            Debug.Log("This is heatseekign");
            bullet.heatSeeking = true;
            rb.useGravity = false;
            
            bullet.defaultTargetPosition = bullet.transform.position + bullet.transform.forward * 100;

            bullet.heatSeekingMini = heatMiniMissle;
            if (chickenType.currentRank == 3 && !heatMiniMissle)
            {
                chickenSizeScale = 3f;
                bullet.heatSeekingSpawner.Start(2f);
                rb.GetComponent<SphereCollider>().enabled = false;
            }

            if(heatMiniMissle)
            {
                chickenSizeScale = 1f;
                rb.GetComponent<SphereCollider>().isTrigger = false;
            }

            

        }
        else
        {
            bullet.heatSeeking = false;
            rb.AddForce(bulletObj.transform.forward * rb.mass * 4000);
        }

        bulletObj.transform.localScale *= chickenSizeScale;

        // Debug code to see where bullet is targeting
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = targetPosition;

        return bulletObj;
    }


}
