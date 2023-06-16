using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TusukanMematikan : Skill
{
    public TusukanMematikan()
    {
        this.skillName = "Tusukan Mematikan";
        this.mpCost = 15;
        this.baseDamage = 250;
        this.target = Target.ENEMY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead() && target.targetable)
        {
            caster.MP -= mpCost;
            caster.animator.SetTrigger("3stab");
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
            if (damage == -1) return -3;
            damage = (int)((float)damage * 100/(100 - target.defense));
            target.TakeDamage(caster, damage);
            return damage;
        }
        return -2;
    }
}
