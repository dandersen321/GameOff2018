using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChickenType", menuName = "ChickenType")]
public class ChickenType : ScriptableObject {

    public int count;
    public int baseDamage;
    public float cooldown;
    public Sprite sprite;
    //public string chickenName;
    //public List<ChickenEffect> chickenEffects;

}
