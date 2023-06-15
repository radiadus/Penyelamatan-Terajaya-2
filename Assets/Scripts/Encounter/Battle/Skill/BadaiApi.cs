using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadaiApi : Skill
{
    public BadaiApi()
    {
        this.skillName = "Badai Api";
        this.mpCost = 25;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("2hand");
        int potency = (int)(caster.GetAttack() * 0.5f);
        foreach(CombatUnit target in targets)
        {
            if (!target.IsDead())
            {
                new Burn(3, potency, target);
            }
        }
        return -1;
    }
}
