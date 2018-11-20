using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour {

    public bool startDayTime = true;

    public TimeOfDayTransition dayTime;
    public TimeOfDayTransition nightTime;

    public Light sun;

    private void OnEnable()
    {
        References.GetTurrent().OnTurrentActiveChange += TurrentActive;
    }

    private void OnDisable()
    {
        References.GetTurrent().OnTurrentActiveChange -= TurrentActive;
    }

    private void TurrentActive(bool active)
    {
        if (active)
            nightTime.PerformTimeOfDayTransition(sun);
        else
            dayTime.PerformTimeOfDayTransition(sun);
    }

    // Use this for initialization
    void Start () {

        if (startDayTime)
            dayTime.PerformTimeOfDayTransition(sun);
        else
            nightTime.PerformTimeOfDayTransition(sun);
	}
}
