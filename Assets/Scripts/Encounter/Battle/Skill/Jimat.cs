using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jimat : Skill
{
    public Jimat()
    {
        this.skillName = "Jimat";
        this.mpCost = 8;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 2;
        this.skillDescription = "Meningkatkan serangan karakter yang diberikan jimat";
        this.clip = Resources.Load<AudioClip>(path + "Warrior Buff");
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (!target.IsDead())
        {
            caster.MP -= mpCost;
            caster.animator.SetTrigger("heal");
            new AttackUp(3, 25, target);
        }
        return -1;
    }
}
