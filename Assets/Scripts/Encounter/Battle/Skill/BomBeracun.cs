using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomBeracun : Skill
{
    public BomBeracun()
    {
        this.skillName = "Bom Beracun";
        this.mpCost = 15;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 2;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("throw");
        foreach(CombatUnit target in targets)
        {
            bool success = Random.Range(0, 100) < 80;
            if (success)
            {
                new Poison(3, 15, target);
            }
        }
        return -1;
    }
}
