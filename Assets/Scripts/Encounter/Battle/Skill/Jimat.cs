using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jimat : Skill
{
    public Jimat()
    {
        this.skillName = "Jimat";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 2;
    }

    public override void Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        base.Cast(caster, targets);
        int damage = (int)(caster.GetAttack() * (baseDamage / 100) * Random.Range(0.95f, 1.05f));
        CombatUnit target = targets[0];
        target.TakeDamage(damage);
    }
}