using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provokasi : Skill
{
    public Provokasi()
    {
        this.skillName = "Provokasi";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
