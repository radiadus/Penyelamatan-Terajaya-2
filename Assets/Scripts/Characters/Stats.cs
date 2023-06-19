using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Stats : ScriptableObject
{
    public int HP, maxHP;
    public int MP, maxMP;
    public int attack;
    public int defense;
    public int speed;
    public int level, exp, requiredExp;
    public int hpGrowth, mpGrowth, attackGrowth, defenseGrowth, speedGrowth;
    public int attackBonusLevel, defenseBonusLevel, speedBonusLevel;
    public List<Skill> skillList;
    public bool isDead;
    public Friendly friendly;
    public Equipment equipment;

    public virtual void Reset()
    {
        level = 1;
        exp = 0;
        requiredExp = 100;
    }

    public virtual void FullHeal()
    {
        HP = maxHP;
        MP = maxMP;
    }

    public virtual void InitializeSkills()
    {
        skillList = new List<Skill>();
    }
}
