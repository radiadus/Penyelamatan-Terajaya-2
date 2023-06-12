using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemangatPemuda : Skill
{
    public SemangatPemuda()
    {
        this.skillName = "Semangat Pemuda";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ALLY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
