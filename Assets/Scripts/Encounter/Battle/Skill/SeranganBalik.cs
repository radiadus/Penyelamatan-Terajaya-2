using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeranganBalik : Skill
{
    public SeranganBalik()
    {
        this.skillName = "Serangan Balik";
        this.mpCost = 10;
        this.baseDamage = 20;
        this.target = Target.SELF;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("cast");
        new Counter(1, caster);
        return -1;
    }
}
