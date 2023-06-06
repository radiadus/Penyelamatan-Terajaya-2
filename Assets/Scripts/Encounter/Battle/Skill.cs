using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public string skillName;
    public int mpCost;
    public int baseDamage;
    public int difficulty;
    public enum Target
    {
        ALLY,
        ENEMY,
        ALL_ALLY,
        ALL_ENEMY,
        NONE
    }

    public Target target;

    public virtual void Cast(CombatUnit caster, List<CombatUnit> targets)
    {
        caster.MP -= this.mpCost;
    }
}
