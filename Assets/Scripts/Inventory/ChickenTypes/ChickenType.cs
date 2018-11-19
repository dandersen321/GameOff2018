using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChickenType", menuName = "ChickenType")]
public class ChickenType : ScriptableObject {

    public int chickenCount;
    public int baseDamage;
    public float cooldown;
    public Sprite sprite;

    public int seedCount;
    public List<GameObject> seedStages;
    public GameObject grownSeed;
    public GameObject startSeed;
    //public string chickenName;
    //public List<ChickenEffect> chickenEffects;

}
