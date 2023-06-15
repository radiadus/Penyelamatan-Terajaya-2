using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaApi : Skill
{
    public BolaApi()
    {
        this.skillName = "Bola Api";
        this.mpCost = 5;
        this.baseDamage = 100;
        this.target = Target.ENEMY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead())
        {
            caster.MP -= this.mpCost;
            caster.animator.SetTrigger("1hand");
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
            if (damage == -1) return -3;
            target.TakeDamage(caster, damage);
            return damage;
        }
        return -2;
    }
}
