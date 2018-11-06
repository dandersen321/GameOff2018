using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    public float endTime;

    public Timer()
    {
        endTime = 0;
    }

    public void Start(float duration)
    {
        endTime = Time.time + duration;
    }

    public bool Expired()
    {
        return endTime <= Time.time;
    }

}
