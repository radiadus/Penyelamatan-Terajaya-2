using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosisiJaga : Skill
{
    public PosisiJaga()
    {
        this.skillName = "Posisi Jaga";
        this.mpCost = 10;
        this.baseDamage = 20;
        this.target = Target.SELF;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("cast");
        new DefenseUp(3, 50, caster);
        return -1;
    }
}
