using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakratulMaut : Skill
{
    public SakratulMaut()
    {
        this.skillName = "Sakratul Maut";
        this.mpCost = 25;
        this.baseDamage = 20;
        this.target = Target.ENEMY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead() && target.targetable)
        {
            caster.MP -= mpCost;
            caster.animator.SetTrigger("2hand");
            bool success = Random.Range(0, 100) < 50;
            if (success)
            {
                new Doom(2, target);
            }
            return -1;
        }
        return -2;
    }
}
