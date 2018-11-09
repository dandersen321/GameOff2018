using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void use()
    {
        //References.GetPlayer().activateCatapult();
        References.GetPlayerMovementController().setCatapultMode();
    }
}
