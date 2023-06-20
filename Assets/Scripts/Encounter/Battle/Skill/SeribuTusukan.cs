using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeribuTusukan : Skill
{
    public SeribuTusukan()
    {
        this.skillName = "Seribu Tusukan";
        this.mpCost = 0;
        this.baseDamage = 100;
        this.target = Target.ENEMY;
        this.difficulty = 2;
        this.skillDescription = "Menusuk musuh berkali-kali";
        this.clip = Resources.Load<AudioClip>(path + "Assassin 2");
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead() && target.targetable)
        {
            int roll = Random.Range(0, 100);
            int baseDamage = this.baseDamage;
            string trigger;
            if (roll < 50)
            {
                trigger = "stab1";
            }
            else if (roll < 75)
            {
                baseDamage += 75;
                trigger = "stab2";
            }
            else
            {
                baseDamage += 150;
                trigger = "stab3";
            }
            caster.animator.SetTrigger(trigger);
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
            if (damage == -1) return -3;
            target.TakeDamage(caster, damage);
            return damage;
        }
        return -2;
    }
}
