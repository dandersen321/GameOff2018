using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour {

    public bool startDayTime = true;

    public Animator transitionAnimation;
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
        {
            StartCoroutine(StartNightTransition());
        }
        else
        {
            StartCoroutine(StartDayTransition());
        }
    }

    IEnumerator StartNightTransition()
    {
        transitionAnimation.SetTrigger("TurretTransitionStart");
        yield return new WaitForSeconds(1f);
        nightTime.PerformTimeOfDayTransition(sun);
        transitionAnimation.SetTrigger("TurretTransitionEnd");
    }

    IEnumerator StartDayTransition()
    {
        transitionAnimation.SetTrigger("TurretTransitionStart");
        yield return new WaitForSeconds(1.5f);
        dayTime.PerformTimeOfDayTransition(sun);
        transitionAnimation.SetTrigger("TurretTransitionEnd");
    }

    // Use this for initialization
    void Start () {

        if (startDayTime)
        {
            dayTime.PerformTimeOfDayTransition(sun);
        }
        else
        {
            nightTime.PerformTimeOfDayTransition(sun);
        }
	}
}
