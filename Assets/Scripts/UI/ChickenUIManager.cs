using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenUIManager : MonoBehaviour {

    public GameObject chickenEquippedUI;


	// Use this for initialization
	void Start () {
        chickenEquippedUI = GameObject.Find("ChickensEquipped");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
