using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    private AttackData data;
    public Timer attackCooldownTimer = new Timer();
    public Timer attackLengthTimer = new Timer();
    //public float attackLength { get { return data.animationClip.length; } }
    public float attackLength { get { return 1f; } } // TODO
    public float attackRange;

    public Attack(AttackData data, float defaultAttackRange)
    {
        this.data = data;
        attackRange = data.attackRange == 0 ? defaultAttackRange : data.attackRange;
    }

    public void BeginAttack(Animator animator)
    {        attackLengthTimer.Start(attackLength);
        attackCooldownTimer.Start(data.postAttackCooldown);
        //animator.Play(data.animationClip.name.Replace("Clip", "BlendTree"));
    }

    public void DoDamage(Health health)
    {
        health.TakeDamage(data.baseDamage);
    }
}
