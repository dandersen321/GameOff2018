using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References  {

    //// Use this for initialization
    private static Player player;
	//void Start () {
 //       player = GameObject.Find("Player").GetComponent<Player>();

 //   }
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    public static Player GetPlayer()
    {
        // Start race condition?
        //return player;    

        return GameObject.Find("Player").GetComponent<Player>();
    }

    public static PlayerMovementController GetPlayerMovementController()
    {
        return GameObject.Find("Player").GetComponent<PlayerMovementController>();
    }

    public static Catapult GetCatapult()
    {
        return GameObject.Find("Catapult").GetComponent<Catapult>();
    }
}
