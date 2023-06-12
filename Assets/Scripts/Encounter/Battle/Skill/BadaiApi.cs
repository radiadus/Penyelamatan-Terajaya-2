using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadaiApi : Skill
{
    public BadaiApi()
    {
        this.skillName = "Badai Api";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
