using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TebasanLebar : Skill
{
    public TebasanLebar()
    {
        this.skillName = "Tebasan Lebar";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 2;
    }

    public override void Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        base.Cast(caster, targets);
        int damage = (int)(caster.GetAttack() * (baseDamage / 100) * Random.Range(0.95f, 1.05f));
        CombatUnit target = targets[0];
        target.TakeDamage(damage);
    }
}
