using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour {

    public Enemy heldBy = null;
    public bool isInRock = true;
    private Vector3 startingPosition;

	// Use this for initialization
	void Start () {
        startingPosition = this.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetPosition()
    {
        this.transform.position = startingPosition;
        isInRock = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
    }

    public void grab()
    {
        isInRock = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
    }

    public void drop()
    {
        heldBy = null;
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
    }


}
