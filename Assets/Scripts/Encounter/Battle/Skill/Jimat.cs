using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jimat : Skill
{
    public Jimat()
    {
        this.skillName = "Jimat";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
