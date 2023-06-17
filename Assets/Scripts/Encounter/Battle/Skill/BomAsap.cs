using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomAsap : Skill
{
    public BomAsap()
    {
        this.skillName = "Bom Asap";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.ALL_ENEMY;
        this.difficulty = 1;
        this.skillDescription = "Menurunkan akurasi serangan semua musuh";
        this.clip = Resources.Load<AudioClip>(path + "Smoke");
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("throw");
        foreach(CombatUnit target in targets)
        {
            bool success = Random.Range(0, 100) < 50;
            if (success)
            {
                new AccuracyDown(3, 20, target);
                target.animator.SetTrigger("hit");
            }
        }
        return -1;
    }
}
