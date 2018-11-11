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

	void Start () {
        lifeTimer.Start(lifeTime);
        gravityTimer.Start(timeUntilGravity);
    }
	
	void Update ()
    {
        if (lifeTimer.Expired())
        {
            Destroy(this.gameObject);
        }
        if(gravityTimer.Expired())
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
            return;
        Debug.Log("Hit " + collision.gameObject.name);
        if (!bulletSpent)
        {
            bulletSpent = true;
        }

        Destroy(this.gameObject);
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
