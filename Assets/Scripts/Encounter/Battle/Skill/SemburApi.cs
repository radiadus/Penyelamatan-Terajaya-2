using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemburApi : Skill
{
    public SemburApi()
    {
        this.skillName = "Sembur Api";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ENEMY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
