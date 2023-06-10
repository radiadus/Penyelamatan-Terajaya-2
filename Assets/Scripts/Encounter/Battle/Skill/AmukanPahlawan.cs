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

    public override void Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        base.Cast(caster, targets);
        int damage = (int)(caster.GetAttack() * (baseDamage / 100) * Random.Range(0.95f, 1.05f));
        CombatUnit target = targets[0];
        target.TakeDamage(damage);
    }
}
