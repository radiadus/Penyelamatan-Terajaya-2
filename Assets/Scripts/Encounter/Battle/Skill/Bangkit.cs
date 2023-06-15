using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bangkit : Skill
{
    public Bangkit()
    {
        this.skillName = "Bangkit";
        this.mpCost = 35;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (target.IsDead())
        {
            target.HP = (int)(target.maxHP * 0.25f);
            target.animator.SetBool("isDead", false);
            return -1;
        }
        return -2;
    }
}
