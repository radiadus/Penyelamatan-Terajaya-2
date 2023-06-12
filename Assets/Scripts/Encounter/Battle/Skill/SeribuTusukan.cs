using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeribuTusukan : Skill
{
    public SeribuTusukan()
    {
        this.skillName = "Seribu Tusukan";
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
