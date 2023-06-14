using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaApi : Skill
{
    public BolaApi()
    {
        this.skillName = "Bola Api";
        this.mpCost = 5;
        this.baseDamage = 1000;
        this.target = Target.ENEMY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= this.mpCost;
        caster.animator.SetTrigger("1hand");
        int damage = (int)(caster.GetAttack() * (float)((float)baseDamage / 100) * Random.Range(0.95f, 1.05f));
        CombatUnit target = targets[0];
        if (!target.IsDead())
        {
            target.TakeDamage(caster, damage);
            return damage;
        }
        return -2;
    }
}
