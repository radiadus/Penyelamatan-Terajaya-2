using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debus : Skill
{
    public Debus()
    {
        this.skillName = "Debus";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.SELF;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
