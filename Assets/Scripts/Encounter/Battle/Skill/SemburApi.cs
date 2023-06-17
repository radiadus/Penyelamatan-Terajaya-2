using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemburApi : Skill
{
    public SemburApi()
    {
        this.skillName = "Sembur Api";
        this.mpCost = 10;
        this.baseDamage = 120;
        this.target = Target.ENEMY;
        this.difficulty = 2;
        this.skillDescription = "Menyerang lawan dan memiliki kemungkinan membuat lawan terbakar selama beberapa giliran";
        this.clip = Resources.Load<AudioClip>(path + "Mage 2");
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
                int potency = (int)(caster.GetAttack() * 0.3f);
                new Burn(3, potency, target);
            }
            return damage;
        }
        return -2;
    }
}
