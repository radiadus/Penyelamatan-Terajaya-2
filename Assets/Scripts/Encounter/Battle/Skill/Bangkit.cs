using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bangkit : Skill
{
    public Bangkit()
    {
        this.skillName = "Bangkit";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
