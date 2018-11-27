using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DayDialog  {
    public int day;
    public Line[] lines;
}

[System.Serializable]
public struct Line
{
    public string name;
    [TextArea(3, 10)]
    public string line;
}
