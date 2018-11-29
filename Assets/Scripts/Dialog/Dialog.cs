using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DayDialog  {
    [HideInInspector]
    public bool dialogRead = false;
    public int day;
    public List<Line> lines;

    public DayDialog()
    {
        lines = new List<Line>();
    }
}

[System.Serializable]
public class Line
{
    public string name;
    [TextArea(3, 10)]
    public string line;
}
