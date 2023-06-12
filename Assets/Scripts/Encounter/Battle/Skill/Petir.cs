using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petir : Skill
{
    public Petir()
    {
        this.skillName = "Petir";
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
