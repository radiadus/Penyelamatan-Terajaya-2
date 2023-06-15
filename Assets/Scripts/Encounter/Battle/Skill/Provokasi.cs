using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provokasi : Skill
{
    public Provokasi()
    {
        this.skillName = "Provokasi";
        this.mpCost = 5;
        this.baseDamage = 0;
        this.target = Target.SELF;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        new Provoke(caster);
        caster.animator.SetTrigger("cast");
        return 0;
    }
}
