using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomMolotov : Skill
{
    public BomMolotov()
    {
        this.skillName = "Bom Molotov";
        this.mpCost = 20;
        this.baseDamage = 50;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 3;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("throw");
        int totalDamage = 0;
        int potency = (int)(caster.GetAttack() * 0.5f);
        foreach(CombatUnit target in targets)
        {
            if (!target.IsDead())
            {
                int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
                if (damage != -1)
                {
                    totalDamage += damage;
                    target.TakeDamage(caster, damage);
                    bool success = Random.Range(0, 100) < 80;
                    if (success) new Burn(3, potency, target);
                }

            }
        }
        return totalDamage;
    }
}
