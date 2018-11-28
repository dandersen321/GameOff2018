using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "EnemyType")]
public class EnemyType : ScriptableObject {
    public GameObject enemyPrefab;
    public string enemyName;
    //public float baseDamage;
}
