using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory {

	public GameObject createBullet(GameObject bulletObj, Vector3 startPosition, Vector3 targetPosition, ChickenType chickenType)
    {
        GameObject bullet = GameObject.Instantiate(bulletObj);
        bullet.SetActive(true);
        bullet.transform.position = startPosition;
        bullet.transform.LookAt(targetPosition);

        var rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bullet.transform.forward * rb.mass * 4000);

        bullet.AddComponent<Bullet>();
        bullet.GetComponent<Bullet>().init(chickenType);
        
        
        // Debug code to see where bullet is targeting
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = targetPosition;

        return bullet;
    }


}
