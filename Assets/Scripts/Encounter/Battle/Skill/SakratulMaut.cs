using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakratulMaut : Skill
{
    public SakratulMaut()
    {
        this.skillName = "Sakratul Maut";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ENEMY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
