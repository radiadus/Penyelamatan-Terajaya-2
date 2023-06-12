using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TusukanMematikan : Skill
{
    public TusukanMematikan()
    {
        this.skillName = "Tusukan Mematikan";
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
