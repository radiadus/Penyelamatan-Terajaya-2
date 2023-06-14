using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3K : Skill
{
    public P3K()
    {
        this.skillName = "P3K";
        this.mpCost = 5;
        this.baseDamage = 40;
        this.target = Target.SELF;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        int heal = caster.maxHP - caster.HP;
        heal = heal < 40 ? heal : 40;
        caster.HP += heal;
        caster.animator.SetTrigger("cast");
        return heal;
    }
}
