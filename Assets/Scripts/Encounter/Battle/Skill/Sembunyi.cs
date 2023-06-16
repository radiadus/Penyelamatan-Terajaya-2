using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sembunyi : Skill
{
    public Sembunyi()
    {
        this.skillName = "Sembunyi";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.priority = 1;
        this.target = Target.SELF;
        this.difficulty = 1;
        this.skillDescription = "Pembunuh tidak dapat diserang selama 1 giliran";
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("throw");
        new Untargetable(1, caster);
        return -1;
    }
}
