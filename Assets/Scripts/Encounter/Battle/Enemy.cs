using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CombatUnit
{
    public int id;
    public override void PlayDeadAnimation()
    {
        if (animator!= null)
        {
            if (animator.runtimeAnimatorController!= null)
            {
                animator.SetBool("isDead", true);
            }
        }
    }

    public abstract Enemy InitializeStats();

    public abstract void Attack(CombatUnit user, List<CombatUnit> target);
}
