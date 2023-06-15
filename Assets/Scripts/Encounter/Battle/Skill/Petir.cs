using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petir : Skill
{
    public Petir()
    {
        this.skillName = "Petir";
        this.mpCost = 10;
        this.baseDamage = 150;
        this.target = Target.ENEMY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead() && target.targetable)
        {
            caster.MP -= mpCost;
            caster.animator.SetTrigger("2hand");
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
            if (damage == -1) return -3;
            target.TakeDamage(caster, damage);
            bool success = Random.Range(0, 100) < 50;
            if (success)
            {
                new AttackDown(3, 20, target);
            }
            return damage;
        }
        return -2;
    }
}
