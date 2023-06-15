using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TebasanLebar : Skill
{
    public TebasanLebar()
    {
        this.skillName = "Tebasan Lebar";
        this.mpCost = 0;
        this.baseDamage = 50;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("swordslash");
        int totalDamage = 0;
        foreach(CombatUnit target in targets)
        {
            if (!target.IsDead() && target.targetable)
            {
                int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
                totalDamage += damage == -1 ? 0 : damage;
            }
        }
        return totalDamage;
    }
}
