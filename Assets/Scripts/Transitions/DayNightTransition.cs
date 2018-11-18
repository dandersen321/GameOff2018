using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour {

    public bool startDayTime = true;


    public Material dayTimeSkyBox;
    public Color dayTimeSkyColor;
    public Color dayTimeEquatorColor;
    public Color dayTimeGroundColor;

    public Material nightTimeSkyBox;
    public Color nightTimeSkyColor;
    public Color nightTimeEquatorColor;
    public Color nightTimeGroundColor;


    public Light sun;

    public Vector3 sunDayAngle;
    public Vector3 sunNightAngle;

    public Color sunDayColor;
    public Color sunNightColor;

    public float sunIntesityDay = 1;
    public float sunIntesityNight = 0.5f;


    public void PerformDayTransition()
    {
        sun.transform.rotation = Quaternion.Euler(euler: sunDayAngle);
        sun.color = sunDayColor;
        sun.intensity = sunIntesityDay;

        RenderSettings.ambientSkyColor = dayTimeSkyColor;
        RenderSettings.ambientEquatorColor = dayTimeEquatorColor;
        RenderSettings.ambientGroundColor = dayTimeGroundColor;

        RenderSettings.skybox = dayTimeSkyBox;
    }

    public void PerformNightTransition()
    {
        sun.transform.rotation = Quaternion.Euler(euler: sunNightAngle);
        sun.color = sunNightColor;
        sun.intensity = sunIntesityNight;

        RenderSettings.ambientSkyColor = nightTimeSkyColor;
        RenderSettings.ambientEquatorColor = nightTimeEquatorColor;
        RenderSettings.ambientGroundColor = nightTimeGroundColor;

        RenderSettings.skybox = nightTimeSkyBox;
    }

	// Use this for initialization
	void Start () {

        if (startDayTime)
            PerformDayTransition();
        else
            PerformNightTransition();
	}
	
	// Update is called once per frame
	void Update () {

        // Debug code
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Change the time of day to the opposite        
            if (startDayTime)
            {
                PerformNightTransition();
                startDayTime = false;
            }
            else
            {
                PerformDayTransition();
                startDayTime = true;
            }
        }
		
	}
}
