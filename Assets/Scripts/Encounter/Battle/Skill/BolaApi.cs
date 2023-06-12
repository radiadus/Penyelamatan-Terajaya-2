using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaApi : Skill
{
    public BolaApi()
    {
        this.skillName = "Bola Api";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ENEMY;
        this.difficulty = 1;
    }

    public override void Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        base.Cast(caster, targets);
        caster.animator.SetTrigger("1hand");
        int damage = (int)(caster.GetAttack() * (float)((float)baseDamage / 100) * Random.Range(0.95f, 1.05f));
        Debug.Log(damage);
        CombatUnit target = targets[0];
        target.TakeDamage(damage);
    }
}
