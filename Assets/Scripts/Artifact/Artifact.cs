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
    }
}
