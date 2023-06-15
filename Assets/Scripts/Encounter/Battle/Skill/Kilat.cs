using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kilat : Skill
{
    public Kilat()
    {
        this.skillName = "Kilat";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("1hand");
        foreach(CombatUnit target in targets)
        {
            bool success = Random.Range(0, 100) < 50;
            if (success)
            {
                new AccuracyDown(1, 40, target);
                target.animator.SetTrigger("hit");
            }
        }
        return -1;
    }
}
