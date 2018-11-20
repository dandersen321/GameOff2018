using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "timeOfDay", menuName = "TimeOfDay")]
public class TimeOfDayTransition : ScriptableObject {

    public Material skyBox;
    public Color skyColor;
    public Color equatorColor;
    public Color groundColor;

    public Vector3 sunAngle;
    public Color sunColor;
    public float sunIntesity = 1;

    public void PerformTimeOfDayTransition(Light sun)
    {
        sun.transform.rotation = Quaternion.Euler(euler: sunAngle);
        sun.color = sunColor;
        sun.intensity = sunIntesity;

        RenderSettings.ambientSkyColor = skyColor;
        RenderSettings.ambientEquatorColor = equatorColor;
        RenderSettings.ambientGroundColor = groundColor;

        RenderSettings.skybox = skyBox;
    }

}
