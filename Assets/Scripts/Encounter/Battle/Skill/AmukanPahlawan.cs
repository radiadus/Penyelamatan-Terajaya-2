using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmukanPahlawan : Skill
{
    public AmukanPahlawan()
    {
        this.skillName = "Amukan Pahlawan";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.SELF;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        return 0;
    }
}
