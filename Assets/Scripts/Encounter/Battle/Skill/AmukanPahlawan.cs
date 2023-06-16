using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmukanPahlawan : Skill
{
    public AmukanPahlawan()
    {
        this.skillName = "Amukan Pahlawan";
        this.mpCost = 5;
        this.baseDamage = 20;
        this.target = Target.SELF;
        this.difficulty = 3;
        this.skillDescription = "Petarung menurunkan pertahanan sendiri namun meningkatkan serangan selama 3 giliran";
    }

    public override int Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= mpCost;
        caster.animator.SetTrigger("cast");
        new AttackUp(3, 100, caster);
        new DefenseDown(3, 50, caster);
        return -1;
    }
}
