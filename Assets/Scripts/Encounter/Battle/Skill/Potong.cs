using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potong : Skill
{
    public Potong()
    {
        this.skillName = "Potong";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ENEMY;
        this.difficulty = 1;
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.animator.SetTrigger("slash");
        int damage = (int)(caster.GetAttack() * ((float)baseDamage / 100) * Random.Range(0.95f, 1.05f));
        Debug.Log(damage);
        CombatUnit target = targets[0];
        target.TakeDamage(damage);
        return damage;
    }
}
