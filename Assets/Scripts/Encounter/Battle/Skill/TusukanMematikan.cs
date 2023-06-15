using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TusukanMematikan : Skill
{
    public TusukanMematikan()
    {
        this.skillName = "Tusukan Mematikan";
        this.mpCost = 0;
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
            caster.animator.SetTrigger("3stab");
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
        }
        return -2;
    }
}
