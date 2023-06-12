using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TebasanLebar : Skill
{
    public TebasanLebar()
    {
        this.skillName = "Tebasan Lebar";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
