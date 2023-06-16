using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debus : Skill
{
    public Debus()
    {
        this.skillName = "Debus";
        this.mpCost = 5;
        this.baseDamage = 0;
        this.target = Target.SELF;
        this.priority = 1;
        this.difficulty = 1;
        this.skillDescription = "Dukun tidak dapat diserang selama 1 giliran";
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("heal");
        new Untargetable(1, caster);
        return -1;
    }
}
