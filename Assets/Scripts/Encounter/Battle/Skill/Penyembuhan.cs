using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penyembuhan : Skill
{
    public Penyembuhan()
    {
        this.skillName = "Penyembuhan";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
