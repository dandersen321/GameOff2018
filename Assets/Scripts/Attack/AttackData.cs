using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    public AnimationClip animationClip;
    public float attackRange;
    public int baseDamage;
    public float postAttackCooldown;
}
