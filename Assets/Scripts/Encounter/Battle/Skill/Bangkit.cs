using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bangkit : Skill
{
    public Bangkit()
    {
        this.skillName = "Bangkit";
        this.mpCost = 35;
        this.baseDamage = 20;
        this.target = Target.ALLY;
        this.difficulty = 3;
        this.skillDescription = "Membangkitkan karakter lain yang telah mati";
        this.clip = Resources.Load<AudioClip>(path + "Mage Heal");
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        if (target.IsDead())
        {
            caster.MP -= mpCost;
            caster.animator.SetTrigger("heal");
            target.HP = (int)(target.maxHP * 0.25f);
            target.animator.SetBool("isDead", false);
            return -1;
        }
        return -2;
    }
}
