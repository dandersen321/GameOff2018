using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmChicken : MonoBehaviour {

    float speed = 6f;
    public Vector3 landingPositon;
    public Vector3 startPosition;

    DirtPatch dirtPatch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator eatPlant(DirtPatch dirtPatch)
    {
        this.dirtPatch = dirtPatch;
        Vector3 targetPosition = dirtPatch.centerLocation;
        //Debug.Log("Eatting plant!");
        float distanceAway = 7f;
        
        
        
        Vector2 offsetV2 = (Random.insideUnitCircle.normalized * distanceAway);
        Vector3 ufoOffset = new Vector3(offsetV2.x, landingPositon.y + distanceAway, offsetV2.y);
        landingPositon = targetPosition;
        startPosition = landingPositon + ufoOffset;
        this.transform.position = startPosition;

        while (Vector3.Distance(this.transform.position, targetPosition) > 0.25f)
        {
            float step = speed * Time.deltaTime;
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
            yield return new WaitForFixedUpdate();
            //Debug.Log("Distance: " + Vector3.Distance(this.transform.position, targetPosition).ToString());
        }
        yield return new WaitForSeconds(1f);
        ChickenType growingChicken = dirtPatch.growingChickenFood;
        this.GetComponentInChildren<Renderer>().material = growingChicken.chickenColorMaterial;
        dirtPatch.finishPickPlan();
        StartCoroutine(flyAway());
    }

    IEnumerator flyAway()
    {
        //Debug.Log("Flying away!");
        while (Vector3.Distance(this.transform.position, startPosition) > 0.25f)
        {
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, startPosition, step);
            yield return new WaitForFixedUpdate();
        }
        
        Destroy(this.gameObject);
    }
}
