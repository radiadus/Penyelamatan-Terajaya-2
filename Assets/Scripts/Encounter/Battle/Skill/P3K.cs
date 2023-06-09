using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3K : Skill
{
    public P3K()
    {
        this.skillName = "P3K";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.SELF;
        this.difficulty = 1;
    }

    public override void Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        base.Cast(caster, targets);
        int heal = (int)(caster.defense * (baseDamage / 100) * Random.Range(0.95f, 1.05f));
        caster.HP += heal;
        caster.CheckMaxHPMP();
    }
}
