using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potong : Skill
{
    public Potong()
    {
        this.skillName = "Potong";
        this.mpCost = 0;
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
            caster.animator.SetTrigger("slash");
            int damage = (int)(caster.GetAttack() * ((float)baseDamage / 100) * Random.Range(0.95f, 1.05f));
            target.TakeDamage(caster, damage);
            return damage;
        }
        return -2;
    }
}
