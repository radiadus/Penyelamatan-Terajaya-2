using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tusuk : Skill
{
    public Tusuk()
    {
        this.skillName = "Tusuk";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ENEMY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        int damage = (int)(caster.GetAttack() * (baseDamage / 100) * Random.Range(0.95f, 1.05f));
        CombatUnit target = targets[0];
        target.TakeDamage(damage);
        caster.animator.SetTrigger("stab1");
        return damage;
    }
}
