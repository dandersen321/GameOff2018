using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory {

	public GameObject createBullet(GameObject bulletObjPrefab, Vector3 startPosition, Vector3 targetPosition, ChickenType chickenType)
    {
        GameObject bulletObj = GameObject.Instantiate(bulletObjPrefab);
        bulletObj.SetActive(true);
        bulletObj.transform.position = startPosition;
        bulletObj.transform.LookAt(targetPosition);
        bulletObj.transform.localScale*= chickenType.chickenSizeScale;

        

        bulletObj.AddComponent<Bullet>();
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.init(chickenType);
        var rb = bulletObj.GetComponent<Rigidbody>();

        if (chickenType.name == ChickenTypeEnum.heatSeekingName)
        {
            Debug.Log("This is heatseekign");
            bullet.heatSeeking = true;
            rb.useGravity = false;
            bullet.defaultTargetPosition = bullet.transform.position + bullet.transform.forward * 100;
        }
        else
        {
            bullet.heatSeeking = false;
            rb.AddForce(bulletObj.transform.forward * rb.mass * 4000);
        }


        // Debug code to see where bullet is targeting
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = targetPosition;

        return bulletObj;
    }


}
