using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penyembuhan : Skill
{
    public Penyembuhan()
    {
        this.skillName = "Penyembuhan";
        this.mpCost = 10;
        this.baseDamage = 100;
        this.target = Target.ALLY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead())
        {
            caster.MP -= mpCost;
            caster.animator.SetTrigger("heal");
            int heal = (int)((caster.GetAttack() * 0.5f + target.maxHP * 0.3f) * Random.Range(0.95f, 1.05f));
            int max = target.maxHP - target.HP;
            heal = heal > max ? max : heal;
            target.HP += heal;
            return heal;
        }
        return -2;
    }
}
