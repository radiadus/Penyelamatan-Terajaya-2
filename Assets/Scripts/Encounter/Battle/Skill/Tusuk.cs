using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tusuk : Skill
{
    public Tusuk()
    {
        this.skillName = "Tusuk";
        this.mpCost = 0;
        this.baseDamage = 100;
        this.target = Target.ENEMY;
        this.difficulty = 1;
        this.skillDescription = "Pembunuh menusuk lawan";
        this.clip = Resources.Load<AudioClip>(path + "Assassin 2");
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead())
        {
            caster.MP -= this.mpCost;
            caster.animator.SetTrigger("stab1");
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
            target.TakeDamage(caster, damage);
            return damage;
        }
        return -2;
    }
}
