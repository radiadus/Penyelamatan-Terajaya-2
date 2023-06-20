using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisauBeracun : Skill
{
    public PisauBeracun()
    {
        this.skillName = "Pisau Beracun";
        this.mpCost = 0;
        this.baseDamage = 100;
        this.target = Target.ENEMY;
        this.difficulty = 2;
        this.skillDescription = "Menyerang musuh dengan pisau beracun, memiliki kemungkinan membuat musuh terkena racun";
        this.clip = Resources.Load<AudioClip>(path + "Assassin 2");
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead() && target.targetable)
        {
            int damage = CombatUnit.CalculateDamage(caster, target, baseDamage);
            caster.animator.SetTrigger("stab2");
            if (damage == -1) return -3;
            target.TakeDamage(caster, damage);
            bool success = Random.Range(0, 100) < 50;
            if (success)
            {
                new Poison(3, 15, target);
            }
            return damage;
        }
        return -2;
    }
}
