using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationPlayer : MonoBehaviour {


    public List<string> animationTriggerNames;
    public float timeDelay = 10f;

    private Animator animator;
    private float timeToAnimation = 0f;


	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();		
	}
	
	// Update is called once per frame
	void Update () {

        if (timeToAnimation <= 0)
        {
            timeToAnimation = timeDelay;
            animator.SetTrigger(animationTriggerNames[(int)Random.Range(0, animationTriggerNames.Count)]);
        }
        else
            timeToAnimation -= Time.deltaTime;
		
	}
}
